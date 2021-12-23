import { Buyer } from "./buyer";
import { Seller } from "./seller";

export interface Product
{
    productId: string;
    productName: string;
    shortDescription: string;
    detailedDescription: string;
    category: string;
    startingPrice: number;
    bidEndDate: string;
    seller: Seller;
    buyers: Buyer[];
}