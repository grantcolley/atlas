using System.Xml.Linq;

namespace Atlas.Blazor.Models
{
    public class IconOptionItem
    {
        public string? Id { get; set; }
        public string? Display { get; set; }
        public string? Icon { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }


            if (obj is not IconOptionItem iconOptionItem)
            {
                return false;
            }

            return iconOptionItem?.Display == this.Display;
        }

        public override int GetHashCode()
        {
            if (string.IsNullOrWhiteSpace(Display))
            {
                return "empty".GetHashCode();
            }

            return Display.GetHashCode();
        }

        public override string? ToString()
        {
            return Display;
        }
    }
}
