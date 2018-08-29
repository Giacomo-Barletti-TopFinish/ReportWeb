using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models
{
  
    public class RWListItem : IEquatable<RWListItem>
    {
        public RWListItem(string text, string value)
        {
            Text = text;
            Value = value;
        }

        public RWListItem()
        {
            Text = string.Empty;
            Value = string.Empty;
        }

        public string Text { get; set; }

        public string Value { get; set; }

        public override string ToString()
        {
            return Text;
        }

        public bool Equals(RWListItem other)
        {
            if (other == null)
                return false;

            if (this.Value.Trim() == other.Value.Trim())
                return true;
            else
                return false;
        }

    }

    public class RWListItemComparer : IEqualityComparer<RWListItem>
    {
        public bool Equals(RWListItem x, RWListItem y)
        {
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            return x.Value == y.Value;
        }

        public int GetHashCode(RWListItem product)
        {
            //Check whether the object is null 
            if (Object.ReferenceEquals(product, null)) return 0;

            //Get hash code for the Numf field if it is not null. 
            int hashNumf = product.Value == null ? 0 : product.Value.GetHashCode();

            return hashNumf;
        }
    }
}
