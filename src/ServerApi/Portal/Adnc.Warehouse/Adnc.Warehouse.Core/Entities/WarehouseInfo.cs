﻿using Adnc.Core.Shared.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Warehouse.Core.Entities
{
    public class WarehouseInfo : AggregateRoot
    {
        public long? ProductId { protected set; get; }
        public int Qty { protected set; get; }
        public int FreezedQty { protected set; get; }
        public string Shelf { protected set; get; }

        protected WarehouseInfo()
        {
        }

        internal WarehouseInfo(long Id,string shelf)
        {
            this.Id = Id;
            if (string.IsNullOrEmpty(shelf))
                throw new ArgumentException("shelf");
            this.Shelf = shelf;
            this.Qty = 0;
            this.FreezedQty = 0;
        }

        /// <summary>
        /// 冻结库存
        /// </summary>
        /// <param name="needFreezedQty"></param>
        internal void Freeze(int needFreezedQty)
        {
            if (this.Qty < needFreezedQty)
                throw new ArgumentException("Qty<needFreezedQty");
            if (!this.ProductId.HasValue)
                throw new ArgumentException("ProductId");
            this.FreezedQty += needFreezedQty;
            this.Qty -= needFreezedQty;
        }

        /// <summary>
        /// 解冻库存
        /// </summary>
        /// <param name="needUnfreezeQty"></param>
        internal void Unfreeze(int needUnfreezeQty)
        {
            if (this.FreezedQty < needUnfreezeQty)
                throw new ArgumentException("FreezedQty<needUnfreezeQty");
            if (!this.ProductId.HasValue)
                throw new ArgumentException("ProductId");

            this.FreezedQty -= needUnfreezeQty;
            this.Qty += needUnfreezeQty;
        }

        /// <summary>
        /// 出库
        /// </summary>
        /// <param name="qty"></param>
        internal void Outbound(int qty)
        {
            if (this.FreezedQty < qty)
                throw new ArgumentException("FreezedQty<qty");
            if (!this.ProductId.HasValue)
                throw new ArgumentException("ProductId");
            this.FreezedQty -= qty;
        }

        /// <summary>
        /// 入库
        /// </summary>
        /// <param name="qty"></param>
        internal void Inbound(int qty)
        {
            if (qty <= 0)
                throw new ArgumentException("qty <= 0");
            if (!this.ProductId.HasValue)
                throw new ArgumentException("ProductId");
            this.Qty += qty;
        }

        /// <summary>
        /// 入库
        /// </summary>
        /// <param name="qty"></param>
        internal void Bind(int qty)
        {
            if (qty <= 0)
                throw new ArgumentException("qty <= 0");
            if (!this.ProductId.HasValue)
                throw new ArgumentException("ProductId");
            this.Qty += qty;
        }

        /// <summary>
        /// 分配商品
        /// </summary>
        /// <param name="productId"></param>
        internal void SetProductId(long productId)
        {
            if(this.ProductId.HasValue)
                throw new ArgumentException("ProductId");
            this.ProductId = productId;
        }
    }
}