export type TableStatus = 0 | 1 | 2;

export interface Table {
    tableId: number;
    number: number
    tableStatus: TableStatus
}
