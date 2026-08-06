import { Component, OnInit } from '@angular/core';
import { Observable, forkJoin } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule, Location } from '@angular/common';
import { Table } from '../../models/table';
import { TableService } from '../../core/http/table.service';
import { Order } from '../../models/order';
import { Category } from '../../models/category';
import { Product } from '../../models/product';
import { CategoryService } from '../../core/http/category.service';
import { ProductService } from '../../core/http/product.service';
import { OrderService } from '../../core/http/order.service';
import { OrderItemDto } from '../../models/order-item';
import { OrderItemService } from '../../core/http/order-item.service';
import { OrderStatus } from '../../models/order';
import { CreateOrderDto } from '../../models/DTO/createOrder.dto';

interface OrderItemView {
  name: string;
  quantity: number;
  unitPrice: number;
  subtotal: number;
}

interface CartItem {
  id?: number;
  productId: number;
  productName: string;
  quantity: number;
  price: number;
}

@Component({
  selector: 'app-order',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './order.component.html',
  styleUrl: './order.component.scss'
})
export class OrderComponent implements OnInit {
  table: Table | null = null;
  order: Order | null = null;
  orders: Order[] = [];
  tableOrders: Order[] = [];
  tableId!: number;
  categories: Category[] = [];
  OrderStatus = OrderStatus;
  products: Product[] = [];
  selectedCategoryId: number | null = null;
  cartItems: CartItem[] = [];
  showAddItemsModal = false;

  showOnlyToday: boolean = true;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private tableService: TableService,
    private orderService: OrderService,
    private categoryService: CategoryService,
    private productService: ProductService,
    private orderItemService: OrderItemService,
    private location: Location
  ) { }

  ngOnInit(): void {
    this.tableId = Number(this.route.snapshot.paramMap.get('id'));

    // Busca a mesa
    this.tableService.getById(this.tableId).subscribe({
      next: (data) => this.table = data,
      error: (err) => console.error('Erro ao buscar mesa', err)
    });
    this.loadTodayOrders();
  }

  loadTodayOrders(): void {
    this.showOnlyToday = true;
    this.orderService.getOrdersDay().subscribe({
      next: (orders) => {
        this.tableOrders = orders.filter(o => o.tableId === this.tableId);
        this.updateActiveOrder();
      },
      error: (err) => console.error('Erro ao buscar pedidos do dia', err)
    });
  }

  loadAllOrders(): void {
    this.showOnlyToday = false;
    this.orderService.getAll().subscribe({
      next: (orders) => {
        const filtered = orders.filter(o => o.tableId === this.tableId);

        this.tableOrders = filtered.sort((a, b) => {
          const priorityA = this.getStatusPriority(a.orderStatus);
          const priorityB = this.getStatusPriority(b.orderStatus);

          if (priorityA !== priorityB) {
            return priorityA - priorityB;
          }

          const dateA = new Date(a.orderDate).getTime();
          const dateB = new Date(b.orderDate).getTime();

          return dateB - dateA;
        });

        this.updateActiveOrder();
      },
      error: (err) => console.error('Erro ao buscar todos os pedidos', err)
    });
  }

  private getStatusPriority(status: number | OrderStatus): number {
    const statusNumber = Number(status);

    switch (statusNumber) {
      case 0: // Pendente / Aberto / Em preparo (Topo)
        return 1;
      case 1: // Pronto / Completo (Meio)
        return 2;
      case 2: // Finalizado (Fim)
        return 3;
      default:
        return 4;
    }
  }


  private updateActiveOrder(): void {
    this.order = this.tableOrders.find(o => o.orderStatus == 0) ||
      this.tableOrders.find(o => o.orderStatus == 1) ||
      this.tableOrders.find(o => o.orderStatus == 2) || null;
  }

  private reloadOrders(): void {
    if (this.showOnlyToday) {
      this.loadTodayOrders();
    } else {
      this.loadAllOrders();
    }
  }

  get filteredProducts(): Product[] {
    if (this.selectedCategoryId === null) return this.products;
    return this.products.filter(p => p.categoryId === this.selectedCategoryId);
  }

  goBack(): void {
    this.location.back();
  }

  openAddItemsModal(): void {
    this.showAddItemsModal = true;
    this.cartItems = [];

    if (this.categories.length === 0) {
      this.categoryService.getAll().subscribe({
        next: (data) => {
          this.categories = data;
          if (data.length > 0) this.selectedCategoryId = data[0].id;
        },
        error: (err) => console.error('Erro ao buscar categorias', err)
      });
    }

    if (this.products.length === 0) {
      this.productService.getAll().subscribe({
        next: (data) => this.products = data,
        error: (err) => console.error('Erro ao buscar produtos', err)
      });
    }
  }

  onCategorySelect(categoryId: number): void {
    this.selectedCategoryId = categoryId;
  }

  onAddToCart(product: Product): void {
    const existing = this.cartItems.find(i => i.productId === product.id);
    if (existing) {
      existing.quantity += 1;
    } else {
      this.cartItems.push({
        productId: product.id,
        productName: product.name,
        quantity: 1,
        price: product.price
      });
    }
  }

  closeAddItemsModal(): void {
    this.showAddItemsModal = false;
  }

  onConfirmAddItems(): void {
    if (this.cartItems.length === 0) return;

    if (this.order) {
      const requests = this.cartItems.map(item => {
        const dto: OrderItemDto = {
          orderId: this.order!.id,
          productId: item.productId,
          quantity: item.quantity,
          price: item.price
        };
        return this.orderItemService.create(dto);
      });

      forkJoin(requests).subscribe({
        next: () => {
          this.closeAddItemsModal();
          this.reloadOrders();
        },
        error: (err) => console.error('Erro ao adicionar itens', err)
      });
    } else {
      this.createOrder();
    }
  }

  createOrder(): void {
    const dto: CreateOrderDto = {
      tableId: this.tableId,
      orderDate: new Date().toISOString(),
      items: this.cartItems.map(item => ({
        productId: item.productId,
        quantity: item.quantity
      }))
    };

    this.orderService.createOrder(dto).subscribe({
      next: () => {
        this.closeAddItemsModal();
        this.cartItems = [];
        this.reloadOrders();          // atualiza tableOrders e o pedido ativo (this.order)

        // mesma lógica que você já usa no closeOrder pra sincronizar a mesa
        this.tableService.getById(this.tableId).subscribe({
          next: (updatedTable) => this.table = updatedTable,
          error: (err) => console.error('Erro ao atualizar mesa', err)
        });
      },
      error: (err) => console.error('Erro ao criar pedido', err)
    });
  }

  onSelectOrder(selectOrder: Order): void {
    this.order = selectOrder;
  }

  get orderItemsView(): OrderItemView[] {
    if (!this.order) return [];

    const grouped = new Map<string, OrderItemView>();

    this.order.productNames.forEach((name, i) => {
      const quantity = this.order!.quantities[i] ?? 1;
      const unitPrice = this.order!.unitPrice[i] ?? 0;

      if (grouped.has(name)) {
        const existing = grouped.get(name)!;
        existing.quantity += quantity;
        existing.subtotal += quantity * unitPrice;
      } else {
        grouped.set(name, {
          name,
          quantity,
          unitPrice,
          subtotal: quantity * unitPrice
        });
      }
    });

    return Array.from(grouped.values());
  }

  onNewOrderClick(): void {
    this.router.navigate(['/orders/new'], { queryParams: { tableId: this.tableId } });
  }

  statusClass(status?: string | number): string {
    if (status === undefined || status === null) return 'status-warning';

    const s = status.toString().toLowerCase();

    switch (s) {
      case '0':
        return 'status-warning';
      case '1':
        return 'status-success';
      case '2':
        return 'bg-sky-500';
      default:
        return 'status-warning';
    }
  }

  isOrderOpen(status?: number | string | null): boolean {
    if (status === null || status === undefined) return false;
    const s = status.toString();
    return s === '0' || s === '1';
  }

  isNewOrderOpen(table: number): boolean {
    if (table === null || table === undefined) return false;
    const s = table.toString();
    return s === '1';
  }

  statusBadgeClass(status?: string | number): string {
    if (status === undefined || status === null) return 'status-warning';

    const s = status.toString().toLowerCase();

    switch (s) {
      case '0':
        return 'status-warning';
      case '1':
        return 'status-success';
      case '2':
        return 'bg-sky-500';
      default:
        return 'status-warning';
    }
  }

  timeLineLabel(status?: string): string {
    if (status === null || status === undefined) {
      return 'Sem pedido';
    }

    const statusNumber = Number(status);

    switch (statusNumber) {
      case 0:
        return 'ocupada';
      case 1:
        return 'Sem Pedido';
      case 2:
        return 'Ocupada';
      default:
        return 'Livre';
    }
  }

  statusLabel(status?: number): string {
    if (status === null || status === undefined) {
      return 'Ocupada';
    }

    const statusNumber = Number(status);

    switch (statusNumber) {
      case 0:
        return 'ocupada';
      case 1:
        return 'Livre';
      case 2:
        return 'Ocupada';
      default:
        return 'Livre';
    }
  }

  statusLabelOrder(status?: string): string {
    if (status === null || status === undefined) {
      return 'Finalizado';
    }

    const statusNumber = Number(status);

    switch (statusNumber) {
      case 0:
        return 'Em preraro';
      case 1:
        return 'Pronto';
      case 2:
        return 'Finalizado';
      default:
        return 'Finalizado';
    }
  }

  tableBadgeClass(status?: string | number | null): string {
    if (status === undefined || status === null) return 'badge-ghost';

    const s = status.toString();

    switch (s) {
      case '0':
        return 'bg-amber-50 text-amber-900 font-bold';
      case '1':
        return 'bg-emerald-100 text-green-400 font-bold';
      case '2':
        return 'bg-sky-100 text-sky-500 font-bold';
      default:
        return 'badge-ghost';
    }
  }

  closeOrder(order: number): void {
    this.orderService.closeOrder(order).subscribe({
      next: () => {
        if (this.order) {
          this.order.orderStatus = OrderStatus.Finalized;
        }

        this.tableService.getById(this.tableId).subscribe({
          next: (updatedTable) => {
            this.table = updatedTable;
          },
          error: (err) => console.error('Erro ao atualizar status da mesa:', err)
        });

        this.reloadOrders();
      },
      error: (err) => {
        console.error('Erro ao encerrar a ordem de serviço:', err);
      }
    });
  }

  badgeClass(status?: string): string {
    switch (status) {
      case '0': return 'badge badge-sucess';
      case '1': return 'badge badge-warning';
      case '2': return 'badge badge-warning';
      default: return '';
    }
  }
}