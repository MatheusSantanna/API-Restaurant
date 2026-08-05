import { Component, OnInit } from '@angular/core';
import { Observable, forkJoin } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
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
  tableOrders: Order[] = [];
  tableId!: number;
  categories: Category[] = [];
  OrderStatus = OrderStatus;
  products: Product[] = [];
  selectedCategoryId: number | null = null;
  cartItems: CartItem[] = [];
  showAddItemsModal = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private tableService: TableService,
    private orderService: OrderService,
    private categoryService: CategoryService,
    private productService: ProductService,
    private orderItemService: OrderItemService
  ) { }

  ngOnInit(): void {
    this.tableId = Number(this.route.snapshot.paramMap.get('id'));

    this.tableService.getById(this.tableId).subscribe({
      next: (data) => this.table = data,
      error: (err) => console.error('Erro ao buscar mesa', err)
    });

    this.orderService.getAll().subscribe({
      next: (orders) => {
        this.tableOrders = orders.filter(o => o.tableId === this.tableId);
        this.order = this.tableOrders.find(o => o.orderStatus == 0) ||
          this.tableOrders.find(o => o.orderStatus == 1) ||
          this.tableOrders.find(o => o.orderStatus == 2) || null;
      },
      error: (err) => console.error('Erro ao buscar pedidos', err)
    });
  }

  get filteredProducts(): Product[] {
    if (this.selectedCategoryId === null) return this.products;
    return this.products.filter(p => p.categoryId === this.selectedCategoryId);
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
    if (!this.order || this.cartItems.length === 0) return;

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
  }

  private reloadOrders(): void {
    this.orderService.getAll().subscribe({
      next: (orders) => {
        this.tableOrders = orders.filter(o => o.tableId === this.tableId);
        const updated = this.tableOrders.find(o => o.id === this.order?.id);
        this.order = updated || this.order;
      },
      error: (err) => console.error('Erro ao buscar pedidos', err)
    });
  }

  onSelectOrder(selectOrder: Order): void {
    this.order = selectOrder
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
      case '0': // <-- Adicionado para pegar o "orderStatus": 0
        return 'status-warning'; // Amarelo

      case '1':
        return 'status-success'; // Verde

      case '2':
        return 'bg-sky-500'; // Azul / Roxo

      default:
        return 'status-warning'; // Amarelo padrão
    }

  }

  isOrderOpen(status?: number | string | null): boolean {
    if (status === null || status === undefined) return false;

    const s = status.toString();

    // Retorna TRUE se for um status de pedido ativo/aberto
    // Substitua '0' e '1' pelos códigos que o seu backend usa para pedidos abertos
    return s === '0' || s === '1';
  }


  statusBadgeClass(status?: string | number): string {
    if (status === undefined || status === null) return 'status-warning';

    const s = status.toString().toLowerCase();

    switch (s) {
      case '0': // <-- Adicionado para pegar o "orderStatus": 0
        return 'status-warning'; // Amarelo

      case '1':
        return 'status-success'; // Verde

      case '2':
        return 'bg-sky-500'; // Azul / Roxo

      default:
        return 'status-warning'; // Amarelo padrão
    }

  }


  timeLineLabel(status?: string): string {
    if (status === null || status === undefined) {
      return 'Sem pedido';
    }

    // Converte para número para garantir a comparação correta
    const statusNumber = Number(status);

    switch (statusNumber) {
      case 0:
        return 'ocupada';

      case 1:
        return 'Sem Pedido';

      case 2:
        return 'Ocupada';

      default:
        return 'Livre'; // Padrão
    }
  }

  statusLabel(status?: string): string {
    if (status === null || status === undefined) {
      return 'Ocupada';
    }

    // Converte para número para garantir a comparação correta
    const statusNumber = Number(status);

    switch (statusNumber) {
      case 0:
        return 'ocupada';

      case 1:
        return 'Livre';

      case 2:
        return 'Ocupada';

      default:
        return 'Livre'; // Padrão
    }
  }

  statusLabelOrder(status?: string): string {
    if (status === null || status === undefined) {
      return 'Finalizado';
    }

    // Converte para número para garantir a comparação correta
    const statusNumber = Number(status);

    switch (statusNumber) {
      case 0:
        return 'Em preraro';

      case 1:
        return 'Pronto';

      case 2:
        return 'Finalizado';

      default:
        return 'Finalizado'; // Padrão
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
        this.order!.orderStatus = OrderStatus.Finalized;
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