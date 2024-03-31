export class OrderModel {
    id: string;
    orderNo: string;
    total: number;
    orderProducts: OrderProductModel[];
    orderUserInfo: OrderUserModel;
}

export class OrderProductModel {
    id: string;
    productId: string;
    productName: string;
    quantity: number;
    unitPrice: number;
    note: string;
}

export class OrderUserModel {
    eMail: string;
    name: string;
    surname: number;
}
