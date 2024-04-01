
export class OrderInsertRequestModel {
    orderProducts: OrderProductRequestModel[];
}

export class OrderProductRequestModel {
    productId: string;
    quantity: number;
    note: string;
}
