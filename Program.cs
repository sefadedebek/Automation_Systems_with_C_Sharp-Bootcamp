using System;
using System.Collections.Generic;
using OfficeOpenXml;
using System.IO;

public class Program
{
    public static void Main()
    {
        GasStation station = new GasStation();

        CashRegister cashRegister = new CashRegister(station);
        cashRegister.AddEntry("Sale 1", 50.5);
        cashRegister.AddEntry("Sale 2", 30.2);
        cashRegister.AddEntry("Sale 3", 20.7);

        Market market = new Market(station);
        market.AddProduct("Product A", 10.0);
        market.AddProduct("Product B", 20.0);
        market.AddProduct("Product C", 15.0);
        market.AddProduct("Product D", 25.0);

        market.SellProducts();

        station.SaveToExcel("sales_data.xlsx");
    }
}

public class GasStation
{
    private List<string> registerEntries = new List<string>();
    private double totalSales = 0.0;

    public void AddEntry(string entry, double amount)
    {
        registerEntries.Add($"{entry},{amount}");
    }

    public void CalculateTotalSales()
    {
        foreach (var entry in registerEntries)
        {
            double amount = Convert.ToDouble(entry.Split(',')[1]);
            totalSales += amount;
        }
    }

    public void SaveToExcel(string fileName)
    {
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Register Entries");
            int row = 1;
            foreach (var entry in registerEntries)
            {
                worksheet.Cells[row, 1].Value = entry;
                row++;
            }
            worksheet.Cells[row, 1].Value = "Total";
            worksheet.Cells[row, 2].Value = totalSales;

            FileInfo excelFile = new FileInfo(fileName);
            package.SaveAs(excelFile);
        }
    }
}

public class CashRegister
{
    private GasStation station;

    public CashRegister(GasStation station)
    {
        this.station = station;
    }

    public void AddEntry(string description, double amount)
    {
        station.AddEntry($"{description},{amount}");
    }
}

public class Market
{
    private Dictionary<string, double> products = new Dictionary<string, double>();
    private GasStation station;

    public Market(GasStation station)
    {
        this.station = station;
    }

    public void AddProduct(string name, double price)
    {
        products.Add(name, price);
    }

    public void SellProducts()
    {
        Random rand = new Random();

        foreach (var product in products)
        {
            double quantity = rand.Next(1, 10);
            double totalAmount = product.Value * quantity;
            station.AddEntry($"Sale: {product.Key}, Quantity: {quantity}, Total: {totalAmount}", totalAmount);

            products[product.Key] -= quantity;
        }
    }
}
