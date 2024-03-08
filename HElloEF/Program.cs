using HElloEF;
using HElloEF.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

//TestCreateEmployee();
//TestUpdateEmployee();
//TestRemoveEmployee();
//TestCountEmployees();
//TestUpdateAllEmployees();
//TestDeleteAllEmployees();

void TestDeleteAllEmployees() {
    Enumerable.Range(0, 10).ToList().ForEach(i => TestCreateEmployee());

    NorthwindContext context = new();
    context.Employees.Where(e => e.EmployeeId > 10).ExecuteDelete();
}

void TestDeleteAllEmployeesBis() {
    Enumerable.Range(0, 10).ToList().ForEach(i => TestCreateEmployee());

    NorthwindContext context = new();
    var ids = context.Employees.Select(e => e.EmployeeId).Where(id => id > 10).ToList();
    ids.ForEach(id =>
        context.Entry(new Employee() { EmployeeId = id }).State = EntityState.Deleted
    );
    context.SaveChanges();
}

void TestUpdateAllEmployees() {
    NorthwindContext context = new();

    context.Employees
        .Where(e => e.EmployeeId < 10)
        .ExecuteUpdate(p => p.SetProperty(
            e => e.Notes, "Maintenant " + DateTime.Now.ToString()
        ));
}

void TestUpdateAllEmployeesBis() {
    NorthwindContext context = new();

    //var ids = context.Employees.Select(e => e.EmployeeId).ToList();
    //var employees = ids.Select(id => {
    //    var e = new Employee() { EmployeeId = id, Notes = "coucou" };
    //    context.Entry(e).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
    //    return e;
    //}).ToList();

    //context.SaveChanges();

    string n = "coucou'totot";
    int nb = context.Database.ExecuteSql($"Update Employees Set Notes = {n}");
    Console.WriteLine("Nombre d'update = " + nb);
}

void TestRemoveEmployee() {
    NorthwindContext context = new();
    Employee moi = context.Employees.Find(10) ?? throw new ArgumentException();
    Console.WriteLine(moi.EmployeeId + " - Avant suppression " + context.Entry(moi).State);
    context.Employees.Remove(moi);
    Console.WriteLine(moi.EmployeeId + " - Après suppression " + context.Entry(moi).State);
    context.SaveChanges();
    Console.WriteLine(moi.EmployeeId + " - Apres SavesChanges " + context.Entry(moi).State);
}

void TestUpdateEmployee() {
    NorthwindContext context = new();
    Employee moi = context.Employees.Find(10) ?? throw new ArgumentException();

    Console.WriteLine(moi.EmployeeId + " - Avant modification " + context.Entry(moi).State);
    moi.City = "Toulouse";
    Console.WriteLine(moi.EmployeeId + " - Apres modification " + context.Entry(moi).State);

    context.SaveChanges();
    Console.WriteLine(moi.EmployeeId + " - Apres SavesChanges " + context.Entry(moi).State);
}

void TestCreateEmployee() {
    Employee moi = new() {
        FirstName = "Olivier",
        LastName = "Astre"
    };

    NorthwindContext context = new();
    Console.WriteLine(moi.EmployeeId + " - Avant Add " + context.Entry(moi).State);

    context.Add(moi);
    Console.WriteLine(moi.EmployeeId + " - Après Add " + context.Entry(moi).State);

    context.SaveChanges();
    Console.WriteLine(moi.EmployeeId + " - Après SaveChanges " + context.Entry(moi).State);
}

void TestCountEmployees() {
    NorthwindContext context = new();
    int nbEmployees = context.Employees/*.ToList()*/.Count();
    Console.WriteLine("Nombre d'employees = " + nbEmployees);
}

//ShowCatalogManualExplicitLoading();
//ShowCatalogExplicitLoading();


//ShowCatalogEarlyInvertLoading();
//ShowCatalogEarlyWithConstraintLoading();

void ShowCatalogEarlyInvertLoading() {
    NorthwindContext context = new();
    var catalog = context.Products
        .Where(p => !p.Discontinued && p.Category != null)
        .Include(p => p.Category)
        .OrderBy(p => p.Category!.CategoryName)
        .GroupBy(p => p.Category!.CategoryName)
        .ToList();
    foreach (var categorie in catalog) {
        //Console.WriteLine(categorie.Key + $" ({categorie.Count()})");
        foreach (var product in categorie) {
            //Console.WriteLine("  " + product.ProductName);
        }
    }
}

void ShowCatalogEarlyWithConstraintLoading() {
    NorthwindContext context = new();
    var categories = context.Categories
        .Include(c => c.Products.Where(p => !p.Discontinued))
        .OrderBy(c => c.CategoryName)
        .ToList();
    foreach (var categorie in categories) {
        //Console.WriteLine(categorie.CategoryName + $" ({categorie.Products.Count})");
        foreach (var product in categorie.Products) {
            //Console.WriteLine("  " + product.ProductName);
        }
    }
}

void ShowCatalogEarlyLoading() {
    NorthwindContext context = new();
    var categories = context.Categories.Include(c => c.Products).OrderBy(c => c.CategoryName).ToList();
    foreach (var categorie in categories) {
        categorie.Products = categorie.Products.Where(p => !p.Discontinued).ToList();
        Console.WriteLine(categorie.CategoryName + $" ({categorie.Products.Count})");
        foreach (var product in categorie.Products) {
            Console.WriteLine("  " + product.ProductName);
        }
    }
}

void ShowCatalogManualExplicitLoading() {
    NorthwindContext context = new();
    var categories = context.Categories.OrderBy(c => c.CategoryName).ToList();
    foreach (var categorie in categories) {
        categorie.Products = context.Products
            .Where(p => !p.Discontinued && p.CategoryId == categorie.CategoryId)
            .ToList();
        Console.WriteLine(categorie.CategoryName + $" ({categorie.Products.Count})");
        foreach (var product in categorie.Products) {
            Console.WriteLine("  " + product.ProductName);
        }
    }
}

void ShowCatalogExplicitLoading() {
    NorthwindContext context = new();
    var categories = context.Categories.OrderBy(c => c.CategoryName).ToList();
    foreach (var categorie in categories) {
        context.Entry(categorie).Collection(c => c.Products).Load();
        Console.WriteLine(categorie.CategoryName +
            $" ({categorie.Products.Count(p => !p.Discontinued)})");
        foreach (var product in categorie.Products.Where(p => !p.Discontinued)) {
            Console.WriteLine("  " + product.ProductName);
        }
    }
}

//ShowCatalogLazyLoading();

void ShowCatalogLazyLoading() {
    DbContextOptionsBuilder<NorthwindContext> builder = new();
    builder
        .LogTo(Console.WriteLine, LogLevel.Information)
        .EnableSensitiveDataLogging(true)
        .UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Northwind")
        .UseLazyLoadingProxies(true);

    NorthwindContext context = new(builder.Options);
    var categories = context.Categories.AsNoTracking().OrderBy(c => c.CategoryName).ToList();
    foreach (var categorie in categories) {
        Console.Write(categorie.CategoryName + " (O/n) ?");
        if (Console.ReadLine()?.ToLower() == "n") { continue; }
        Console.WriteLine(categorie.CategoryName +
           $" ({categorie.Products.Count(p => !p.Discontinued)})");
        foreach (var product in categorie.Products.Where(p => !p.Discontinued)) {
            Console.WriteLine("  " + product.ProductName);
        }
    }
}

ShowStats();

void ShowStats() {
    // Le produit le plus vendu
    NorthwindContext context = new();

    //Product? product = context.Products
    //    .MaxBy(p => p.OrderDetails.Sum(od => od.Quantity));
    var p1 = context.Products
        .OrderByDescending(p => p.OrderDetails.Sum(od => od.Quantity))
        .Select(p => new { p.ProductName, Total = p.OrderDetails.Sum(od => od.Quantity) })
        .FirstOrDefault();
    if (p1 != null) {
        Console.WriteLine("Produit le plus vendu : " + p1.ProductName + " x" + p1.Total);
    }


    var p2 = context.Products
        .Select(p => new { 
            p.ProductName, 
            Total = p.OrderDetails.Sum(od => od.Quantity * ((double)od.UnitPrice * (1 - od.Discount))) 
        })
        .OrderByDescending(p => p.Total)
        .FirstOrDefault();
    if (p2 != null) {
        Console.WriteLine("Produit le plus de CA : " + p2.ProductName + " $" + p2.Total);
    }

    var c1 = context.Orders.Join(
            context.OrderDetails,
            o => o.OrderId,
            od => od.OrderId,
            (o, od) => new { 
                o.OrderId,
                o.Customer.CompanyName, 
                OrderDetailTotal = od.Quantity * ((double)od.UnitPrice * (1 - od.Discount))
            }
        )
        .GroupBy(o => o.CompanyName)
        .Select(g => new {
            CompanyName = g.Key,
            Total = g.Sum(od => od.OrderDetailTotal)
        })
        .OrderByDescending(c => c.Total)
        .FirstOrDefault();
    if (c1 != null) {
        Console.WriteLine("Client le plus de CA : " +c1.CompanyName + " $" + c1.Total );
    }
}