using POSManager.API.Products.Models;

namespace API.Products.Tests;

public class IntegrationTest : IClassFixture<IntegrationTestServerFixture>
{
    private readonly IntegrationTestServerFixture _integrationTestServerFixture;

    public IntegrationTest(IntegrationTestServerFixture integrationTestServerFixture)
    {
        _integrationTestServerFixture = integrationTestServerFixture;
    }

    [Theory]
    [ClassData(typeof(TheoryCategoriesDemo))]
    public async Task add_categories(Category category)
    {
        var id = Guid.Empty;

        try
        {
            var result = await Helpers.PostAsync<Category, Category>(category, "categories", _integrationTestServerFixture.HttpClientDemo);

            if (result is not null)
                id = result.CategoryId;

            Assert.NotNull(result);
            Assert.IsType<Category>(result);
            Assert.True(result.CategoryId == category.CategoryId);
        }
        finally
        {
            await Helpers.DeleteAsync($"categories/{id}", _integrationTestServerFixture.HttpClientDemo);
        }
    }

    [Theory]
    [ClassData(typeof(TheoryProductsDemo))]
    public async Task add_products(Product product)
    {
        try
        {
            await FillTestCategories();

            var result = await Helpers.PostAsync<Product, Product>(product, "products", _integrationTestServerFixture.HttpClientDemo);

            Assert.NotNull(result);
            Assert.IsType<Product>(result);
            Assert.True(result.ProductId == product.ProductId);
        }
        finally
        {
            await ClearTestCategories();
        }
    }

    [Fact]
    public async Task get_all_demo_categories_without_products()
    {
        try
        {
            await FillTestData();

            var result = await Helpers.GetAsync<IEnumerable<Category>>("/categories", _integrationTestServerFixture.HttpClientDemo);

            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.True(result.Count() == 7);
            Assert.True(result.All(x => x.Description.EndsWith("Demo")));
            Assert.NotNull(result?.FirstOrDefault()?.Products);
            Assert.False(result?.FirstOrDefault()?.Products.Any());
        }
        finally
        {
            await ClearTestData();
        }
    }

    [Fact]
    public async Task get_all_demo_categories_with_products()
    {
        await FillTestData();

        try
        {
            var result = await Helpers.GetAsync<IEnumerable<Category>>("/categories/true", _integrationTestServerFixture.HttpClientDemo);

            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.True(result.Count() == 7);
            Assert.True(result.All(x => x.Description.EndsWith("Demo")));
            Assert.NotNull(result?.FirstOrDefault()?.Products);
            Assert.True(result.FirstOrDefault()?.Products.Any());
        }
        finally
        {
            await ClearTestData();
        }
    }

    [Fact]
    public async Task get_all_joe_categories_without_products()
    {
        await FillTestData();

        try
        {
            var result = await Helpers.GetAsync<IEnumerable<Category>>("/categories", _integrationTestServerFixture.HttpClientJoe);

            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.True(result.Count() == 7);
            Assert.True(result.All(x => x.Description.EndsWith("Joe")));
            Assert.NotNull(result?.FirstOrDefault()?.Products);
            Assert.False(result?.FirstOrDefault()?.Products.Any());
        }
        finally
        {
            await ClearTestData();
        }
    }

    [Fact]
    public async Task get_all_joe_categories_with_products()
    {
        await FillTestData();

        try
        {
            var result = await Helpers.GetAsync<IEnumerable<Category>>("/categories/true", _integrationTestServerFixture.HttpClientJoe);

            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.True(result.Count() == 7);
            Assert.True(result.All(x => x.Description.EndsWith("Joe")));
            Assert.NotNull(result?.FirstOrDefault()?.Products);
            Assert.True(result.FirstOrDefault()?.Products.Any());
        }
        finally
        {
            await ClearTestData();
        }
    }

    [Fact]
    public async Task get_one_demo_category()
    {
        await FillTestData();

        try
        {
            var categories = await Helpers.GetAsync<IEnumerable<Category>>("/categories", _integrationTestServerFixture.HttpClientDemo);
            var id = categories?.FirstOrDefault()?.CategoryId ?? Guid.Empty;

            var result = await Helpers.GetAsync<Category>($"/categories/{id}", _integrationTestServerFixture.HttpClientDemo);

            Assert.NotNull(result);
            Assert.IsType<Category>(result);
            Assert.EndsWith("Demo", result.Description);
            Assert.True(result.CategoryId == id);
        }
        finally
        {
            await ClearTestData();
        }
    }

    [Fact]
    public async Task get_one_joe_category()
    {
        await FillTestData();

        try
        {
            var categories = await Helpers.GetAsync<IEnumerable<Category>>("/categories", _integrationTestServerFixture.HttpClientJoe);
            var id = categories?.FirstOrDefault()?.CategoryId ?? Guid.Empty;

            var result = await Helpers.GetAsync<Category>($"/categories/{id}", _integrationTestServerFixture.HttpClientJoe);

            Assert.NotNull(result);
            Assert.IsType<Category>(result);
            Assert.EndsWith("Joe", result.Description);
            Assert.True(result.CategoryId == id);
        }
        finally
        {
            await ClearTestData();
        }
    }

    [Fact]
    public async Task get_all_products()
    {
        await FillTestData();

        try
        {
            var result = await Helpers.GetAsync<IEnumerable<Product>>("/products", _integrationTestServerFixture.HttpClientDemo);

            Assert.NotNull(result);
            Assert.True(result.Any());
        }
        finally
        {
            await ClearTestData();
        }
    }

    [Fact]
    public async Task get_one_product()
    {
        await FillTestData();

        try
        {
            var products = await Helpers.GetAsync<IEnumerable<Product>>("/products", _integrationTestServerFixture.HttpClientDemo);
            var id = products?.FirstOrDefault()?.ProductId ?? Guid.Empty;

            var result = await Helpers.GetAsync<Product>($"/products/{id}", _integrationTestServerFixture.HttpClientDemo);

            Assert.NotNull(result);
            Assert.IsType<Product>(result);
            Assert.True(result.ProductId == id);
        }
        finally
        {
            await ClearTestData();
        }
    }


    private async Task FillTestCategories()
    {
        foreach (var category in CategoriesDemo)
        {
            await Helpers.PostAsync<Category, Category>(category, "categories", _integrationTestServerFixture.HttpClientDemo);
        }

        foreach (var category in CategoriesJoe)
        {
            await Helpers.PostAsync<Category, Category>(category, "categories", _integrationTestServerFixture.HttpClientJoe);
        }
    }

    private async Task ClearTestCategories()
    {
        foreach (var category in CategoriesDemo)
        {
            await Helpers.DeleteAsync($"categories/{category.CategoryId}", _integrationTestServerFixture.HttpClientDemo);
        }

        foreach (var category in CategoriesJoe)
        {
            await Helpers.DeleteAsync($"categories/{category.CategoryId}", _integrationTestServerFixture.HttpClientJoe);
        }
    }



    private async Task FillTestData()
    {
        foreach (var category in CategoriesDemo)
        {
            await Helpers.PostAsync<Category, Category>(category, "categories", _integrationTestServerFixture.HttpClientDemo);
        }

        foreach (var category in CategoriesJoe)
        {
            await Helpers.PostAsync<Category, Category>(category, "categories", _integrationTestServerFixture.HttpClientJoe);
        }

        foreach (var product in ProductsDemo)
        {
            await Helpers.PostAsync<Product, Product>(product, "products", _integrationTestServerFixture.HttpClientDemo);
        }

        foreach (var product in ProductsJoe)
        {
            await Helpers.PostAsync<Product, Product>(product, "products", _integrationTestServerFixture.HttpClientJoe);
        }
    }

    private async Task ClearTestData()
    {
        foreach (var category in CategoriesDemo)
        {
            await Helpers.DeleteAsync($"categories/{category.CategoryId}", _integrationTestServerFixture.HttpClientDemo);
        }

        foreach (var category in CategoriesJoe)
        {
            await Helpers.DeleteAsync($"categories/{category.CategoryId}", _integrationTestServerFixture.HttpClientJoe);
        }
    }

    class TheoryCategoriesDemo : TheoryData<Category>
    {
        public TheoryCategoriesDemo()
        {
            foreach (var category in CategoriesDemo)
                Add(category);
        }
    }

    class TheoryProductsDemo : TheoryData<Product>
    {
        public TheoryProductsDemo()
        {
            foreach (var product in ProductsDemo)
                Add(product);
        }
    }


    class TheoryCategoriesJoe : TheoryData<Category>
    {
        public TheoryCategoriesJoe()
        {
            foreach (var category in CategoriesJoe)
                Add(category);
        }
    }

    class TheoryProductsJoe : TheoryData<Product>
    {
        public TheoryProductsJoe()
        {
            foreach (var product in ProductsJoe)
                Add(product);
        }
    }



    public static Category[] CategoriesDemo => new Category[]
    {
        new Category{ CategoryId = Guid.Parse("7e20140a-e080-4da9-856a-56a5364576f5"),Code = "ST1", Description = "Starters Demo" },
        new Category{ CategoryId = Guid.Parse("a82547e5-1be2-4d48-9871-2a1836db00c8"),Code = "SL1", Description = "Salads Demo" },
        new Category{ CategoryId = Guid.Parse("346ad61f-abe4-4988-8e59-115d30ac824e"),Code = "SN1", Description = "Mains Demo" },
        new Category{ CategoryId = Guid.Parse("640f2c6e-0027-435d-8f7f-977d6327bacc"),Code = "DS1", Description = "Deserts Demo" },
        new Category{ CategoryId = Guid.Parse("bc171c8d-efc2-485e-b22f-dbd35b1aff5d"),Code = "BTC", Description = @"Teas & Coffee Demo" },
        new Category{ CategoryId = Guid.Parse("670c442b-b556-41b9-bbe7-3562ca15853f"),Code = "BBR", Description = "Beers Demo" },
        new Category{ CategoryId = Guid.Parse("0279fa61-7e98-4b47-a7f1-d57861c18ce1"), Code = "BSD", Description = "Soft Drinks Demo" }
    };

    public static Product[] ProductsDemo => new Product[]
    {
        new Product{ ProductId = Guid.Parse("9c0e64f1-185d-41dc-af8e-242897599bf9"), CategoryId = Guid.Parse("7e20140a-e080-4da9-856a-56a5364576f5"), Code = "TS", Description = "Tomato Soup Demo"},
        new Product{ ProductId = Guid.Parse("083cafa1-b78e-45ca-99ed-f39e8558ae23"), CategoryId = Guid.Parse("7e20140a-e080-4da9-856a-56a5364576f5"), Code = "BG", Description = "Garlic Bread Demo"},
        new Product{ ProductId = Guid.Parse("91711960-d460-4fdb-a9b8-82181573ab05"), CategoryId = Guid.Parse("a82547e5-1be2-4d48-9871-2a1836db00c8"), Code = "CSS", Description = @"Chef's Special Salad Demo"},
        new Product{ ProductId = Guid.Parse("0cd01ff8-9f36-4f42-8eee-556bbf1728b4"), CategoryId = Guid.Parse("a82547e5-1be2-4d48-9871-2a1836db00c8"), Code = "WS", Description = "Waldorf Salad Demo"},
        new Product{ ProductId = Guid.Parse("4e54b3c9-1a7c-419a-9ed0-d0e8066bf089"), CategoryId = Guid.Parse("346ad61f-abe4-4988-8e59-115d30ac824e"), Code = "NGA", Description = "Nasi Goreng Ayam Demo"},
        new Product{ ProductId = Guid.Parse("05ea13e2-dad3-406f-899e-f0f36ff26f44"), CategoryId = Guid.Parse("346ad61f-abe4-4988-8e59-115d30ac824e"), Code = "TKG", Description = "Tom Kah Gai Demo"},
        new Product{ ProductId = Guid.Parse("00cd7794-05fd-40b5-978d-12ca927f5a4d"), CategoryId = Guid.Parse("640f2c6e-0027-435d-8f7f-977d6327bacc"), Code = "IC", Description = "Ice Cream Demo"},
        new Product{ ProductId = Guid.Parse("de5fdc6c-06ba-495c-a764-aaad76470dbf"), CategoryId = Guid.Parse("640f2c6e-0027-435d-8f7f-977d6327bacc"), Code = "WFL", Description = "Waffles Demo"},
        new Product{ ProductId = Guid.Parse("f3205b34-c828-4fcc-b7af-518ed4f5e4b9"), CategoryId = Guid.Parse("bc171c8d-efc2-485e-b22f-dbd35b1aff5d"), Code = "EBT", Description = "English Breakfast Tea Demo"},
        new Product{ ProductId = Guid.Parse("cfd30e62-3d0e-4d08-a219-92161d0eaae7"), CategoryId = Guid.Parse("bc171c8d-efc2-485e-b22f-dbd35b1aff5d"), Code = "FWC", Description = "Flat White Coffee Demo"},
        new Product{ ProductId = Guid.Parse("852bd97d-c7a1-4fb7-a98a-f987e30b4d0e"), CategoryId = Guid.Parse("670c442b-b556-41b9-bbe7-3562ca15853f"), Code = "SGB", Description = "Singha Beer Demo"},
        new Product{ ProductId = Guid.Parse("85a9606e-d5ed-41d8-bdbf-9d6b8c0a53e9"), CategoryId = Guid.Parse("670c442b-b556-41b9-bbe7-3562ca15853f"), Code = "TB", Description = "Tiger Beer Demo"},
        new Product{ ProductId = Guid.Parse("84dbc869-9fd3-4fcc-8fb4-46a5a51a3032"), CategoryId = Guid.Parse("0279fa61-7e98-4b47-a7f1-d57861c18ce1"), Code = "OJ", Description = "Orange Juice Demo"},
        new Product{ ProductId = Guid.Parse("d427d4eb-a4f1-4f08-a007-e6393208d1ca"), CategoryId = Guid.Parse("0279fa61-7e98-4b47-a7f1-d57861c18ce1"), Code = "CC", Description = "Coke Cola Demo"}
    };

    public static Category[] CategoriesJoe => new Category[]
    {
        new Category{ CategoryId = Guid.Parse("b2738e12-ea87-43aa-869d-9958719313bb"),Code = "ST1", Description = "Starters Joe" },
        new Category{ CategoryId = Guid.Parse("433719fe-69d3-47b7-8241-fe9134157454"),Code = "SL1", Description = "Salads Joe" },
        new Category{ CategoryId = Guid.Parse("30e774c6-e346-4a52-858c-f0993a092aa3"),Code = "SN1", Description = "Mains Joe" },
        new Category{ CategoryId = Guid.Parse("5bca3578-2269-4cb9-8820-af79306ac8ee"),Code = "DS1", Description = "Deserts Joe" },
        new Category{ CategoryId = Guid.Parse("505f62e8-453d-4805-a8da-d7b4620d2ed5"),Code = "BTC", Description = @"Teas & Coffee Joe" },
        new Category{ CategoryId = Guid.Parse("67e6c55e-c905-4db1-b707-26fb94de104d"),Code = "BBR", Description = "Beers Joe" },
        new Category{ CategoryId = Guid.Parse("0c5bd2ec-ac6b-42ad-8532-bef56565297d"), Code = "BSD", Description = "Soft Drinks Joe" }
    };

    public static Product[] ProductsJoe => new Product[]
    {
        new Product{ ProductId = Guid.Parse("99647a87-4ab9-4437-8a6f-690c0f1541cc"), CategoryId = Guid.Parse("b2738e12-ea87-43aa-869d-9958719313bb"), Code = "TS", Description = "Tomato Soup Joe"},
        new Product{ ProductId = Guid.Parse("f08a55d8-59b9-43aa-88a4-c2fb77e0edff"), CategoryId = Guid.Parse("b2738e12-ea87-43aa-869d-9958719313bb"), Code = "BG", Description = "Garlic Bread Joe"},
        new Product{ ProductId = Guid.Parse("536bade0-36b4-4cb9-83ee-27bb582eeb03"), CategoryId = Guid.Parse("433719fe-69d3-47b7-8241-fe9134157454"), Code = "CSS", Description = @"Chef's Special Joe Salad"},
        new Product{ ProductId = Guid.Parse("1c2f97f4-94bd-46c0-ad04-17ac51f89a34"), CategoryId = Guid.Parse("433719fe-69d3-47b7-8241-fe9134157454"), Code = "WS", Description = "Waldorf Salad Joe"},
        new Product{ ProductId = Guid.Parse("ea2bd6f0-dbaa-46bb-a797-9462f181a242"), CategoryId = Guid.Parse("30e774c6-e346-4a52-858c-f0993a092aa3"), Code = "NGA", Description = "Nasi Goreng Ayam Joe"},
        new Product{ ProductId = Guid.Parse("f8386389-83aa-410d-8f32-13ca5e3437b7"), CategoryId = Guid.Parse("30e774c6-e346-4a52-858c-f0993a092aa3"), Code = "TKG", Description = "Tom Kah Gai Joe"},
        new Product{ ProductId = Guid.Parse("614f5c18-98f4-4b7c-af80-56e31d2f4088"), CategoryId = Guid.Parse("5bca3578-2269-4cb9-8820-af79306ac8ee"), Code = "IC", Description = "Ice Cream Joe"},
        new Product{ ProductId = Guid.Parse("526e6ac6-915d-4972-9410-cd11ff90427e"), CategoryId = Guid.Parse("5bca3578-2269-4cb9-8820-af79306ac8ee"), Code = "WFL", Description = "Waffles Joe"},
        new Product{ ProductId = Guid.Parse("46280db0-2bd6-4347-9ce9-3450d73a6c30"), CategoryId = Guid.Parse("505f62e8-453d-4805-a8da-d7b4620d2ed5"), Code = "EBT", Description = "English Breakfast Tea Joe"},
        new Product{ ProductId = Guid.Parse("91ae4120-b97d-4b31-89f8-55683defd630"), CategoryId = Guid.Parse("505f62e8-453d-4805-a8da-d7b4620d2ed5"), Code = "FWC", Description = "Flat White Coffee Joe"},
        new Product{ ProductId = Guid.Parse("fb996e33-c91e-4e82-a74a-01b5667279d2"), CategoryId = Guid.Parse("67e6c55e-c905-4db1-b707-26fb94de104d"), Code = "SGB", Description = "Singha Beer Joe"},
        new Product{ ProductId = Guid.Parse("dc9eed0b-0316-4510-9136-5460960df242"), CategoryId = Guid.Parse("67e6c55e-c905-4db1-b707-26fb94de104d"), Code = "TB", Description = "Tiger Beer Joe"},
        new Product{ ProductId = Guid.Parse("e3b92b1d-30c4-40fa-92e6-4dba84185c9b"), CategoryId = Guid.Parse("0c5bd2ec-ac6b-42ad-8532-bef56565297d"), Code = "OJ", Description = "Orange Juice Joe"},
        new Product{ ProductId = Guid.Parse("b4342f00-1608-48f6-b8db-65289b4f774c"), CategoryId = Guid.Parse("0c5bd2ec-ac6b-42ad-8532-bef56565297d"), Code = "CC", Description = "Coke Cola Joe"}
    };
}

