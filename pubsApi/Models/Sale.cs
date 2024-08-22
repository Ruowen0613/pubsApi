using PubsApi.Models;

public class Sale
{
    public string Stor_id { get; set; }  // Maps to stor_id in SQL
    public string Ord_num { get; set; }  // Maps to ord_num in SQL
    public string Title_id { get; set; }  // Maps to title_id in SQL
    public DateTime Ord_date { get; set; }  // Maps to ord_date in SQL
    public short Qty { get; set; }  // Maps to qty in SQL
    public string Payterms { get; set; }  // Maps to payterms in SQL

    // Navigation properties (if needed)
    //public virtual Store Store { get; set; }  // Optional: If you have a Store entity
    //public virtual Title Title { get; set; }  // Optional: If you have a Title entity
}
