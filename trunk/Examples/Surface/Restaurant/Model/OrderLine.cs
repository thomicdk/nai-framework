using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Restaurant.Model.Menu;

namespace Restaurant.Model
{
    public class OrderLine
    {
        public OrderableItem Item { get; private set; }

        public Person Owner { get; private set; }
        //public Dictionary<Person, float> Payers { get; set; }
        

        //public List<SharedItemPayer> Payers { get; private set; }

        public OrderLine(Person owner, OrderableItem item)
        {
            this.Owner = owner;
            this.Item = item;
            //Payers = new Dictionary<Person, float>();
            //Payers[Owner] = 1f;
        }

        //public void AddPayer(Person newPayer, float share)
        //{
        //    if (share < 0 || share > 1)
        //        throw new ArgumentOutOfRangeException("Argument 'share' must be a number between 0 and 1");
        //    if (share > Payers[Owner])
        //        throw new ArgumentOutOfRangeException("Argument 'share' must be smaller than the share of the owner of this OrderLine");

        //    Payers[Owner] = Payers[Owner] - share;
        //    Payers[newPayer] = share;
        //}

        //public void RemovePayer(Person payer)
        //{
        //    if (Payers[payer].Equals(Owner))
        //        throw new ArgumentException("Cannot remove the owner of the order line from payers");

        //    if (Payers.ContainsKey(payer))
        //    {
        //        Payers[Owner] = Payers[Owner] + Payers[payer];
        //        Payers.Remove(payer);
        //    }
        //}

        //public void TransferOwnerShip(Person newOwner)
        //{ 
        //    TransferPayShare(Owner, newOwner);
        //}

        //public void TransferPayShare(Person oldPayer, Person newPayer)
        //{
        //    if (Payers.ContainsKey(oldPayer))
        //    { 
        //        if (oldPayer.Equals(Owner))
        //        {
        //            this.Owner = newPayer;
        //        }

        //        if (Payers.ContainsKey(newPayer))
        //        {
        //            Payers[newPayer] = Payers[newPayer] + Payers[oldPayer];
        //        }
        //        else
        //        {
        //            Payers[newPayer] = Payers[oldPayer];
        //        }

        //        Payers.Remove(oldPayer);
        //    }
        //}

        public void PayForAll(Person payer)
        {
            //Payers.Clear();
            //Payers[payer] = 1f;
            //this.Owner = payer;
            this.Owner = payer;
        }
    }
}
