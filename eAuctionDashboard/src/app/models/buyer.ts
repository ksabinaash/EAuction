import { Seller } from "./seller";

export interface Buyer extends Seller
 {
    bidAmount: number;
    productId: number;
  }