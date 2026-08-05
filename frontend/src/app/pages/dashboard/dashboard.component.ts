import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Table } from '../../models/table';
import { TableService } from '../../core/http/table.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit {
  table: Table[] = [];

  constructor(private tableService: TableService,
    private router: Router
  ) {}

  onOrderButtonClick(table: Table): void {
  this.router.navigate(['/orders/table', table.tableId]);
}

  ngOnInit(): void {
    this.tableService.getAll().subscribe({
      next: (data) => this.table = data,
      error: (err) => console.error('erro ao buscar mesas', err)
    });
  }


badgeClass(status: string): string {
    switch (status) {
        case '0': return 'badge-success';
        case '1': return 'badge-error';
        case '2': return 'badge-warning';
        default: return '';
     }
    } 
  


btnClass(status: string): string {
    switch (status) {
        case '0': return 'btn-success';
        case '1': return 'btn-error';
        case '2': return 'btn-warning';
        default: return '';
    }
}

btnLabel(status: string): string {
    if (status === null || status === undefined) {
    return 'Novo Pedido';
  }

  // Converte para número para garantir a comparação correta
  const statusNumber = Number(status);
  if(statusNumber != 0){
    return 'Novo Pedido'
  }
  else
    {
      return 'Abrir Pedido'
    }
}

    


statusLabel(status: string): string {
    if (status === null || status === undefined) {
    return 'Ocupada';
  }

  // Converte para número para garantir a comparação correta
  const statusNumber = Number(status);

  switch (statusNumber) {
    case 0:
      return 'Ocupada'; // Amarelo (Pendente / Livre / Espera)

    case 1:
      return 'Livre'; // Verde (Ocupada / Confirmado)

    case 2:
      return 'Reservada'; // Azul/Roxo (Reservada / Em Andamento)

    default:
      return 'Livre'; // Padrão
  }
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
      return 'status-primary'; // Azul / Roxo

    default:
      return 'status-warning'; // Amarelo padrão
  }

}
tableBadgeClass(status?: string | number | null): string {
  if (status === undefined || status === null) return 'badge-ghost';

  const s = status.toString();

  switch (s) {
    case '0':
      return 'bg-amber-100 text-amber-600 font-bold'; // Verde para Livre
    case '1':
      return 'bg-emerald-100 text-green-400 font-bold';   // Vermelho para Ocupada
    case '2':
      return 'badge-warning'; // Amarelo para Reservada
    default:
      return 'badge-ghost';
  }
}


 auraClass(status: string): string {
 if (status === null || status === undefined) {
    return 'text-warning';
  }

  // Converte para número para garantir a comparação correta
  const statusNumber = Number(status);

  switch (statusNumber) {
    case 0:
      return 'text-warning'; // Amarelo (Pendente / Livre / Espera)

    case 1:
      return 'text-green-600'; // Verde (Ocupada / Confirmado)

    case 2:
      return 'text-red-600'; // Azul/Roxo (Reservada / Em Andamento)

    default:
      return 'text-warning'; // Padrão
  }
}
}