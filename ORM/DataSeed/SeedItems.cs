using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ORM.Models;

namespace ORM.DataSeed
{
  public static class SeedItems
  {
    public static async Task SeedAsync(
     ApplicationDbContext context,
     ILogger logger)
    {
      if (await context.Items.AnyAsync())
      {
        logger.LogInformation("Items already exist in database.");
        return;
      }
      else
      {
        logger.LogInformation("Create items in database.");
      }

      await CreateItemsAsync(context, logger);
    }

    private static async Task CreateItemsAsync(ApplicationDbContext context, ILogger logger)
    {
      var items = await GetItemsAsync();

      try
      {
        await context.Items.AddRangeAsync(items);
        await context.SaveChangesAsync();
      }
      catch (DbUpdateException ex)
      {
        logger.LogError($"Can not create items.");
        logger.LogError(ex.InnerException, nameof(SeedItems.SeedAsync));
      }
    }

    private static async Task<IList<Item>> GetItemsAsync()
    {
      var path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\ORM\DataSeed\Content"));

      return new List<Item>
      {
        new Item 
        {
          Title = "item1",
          Price = 20,
          CountsInStock = 10,
          Image = await File.ReadAllBytesAsync(Path.Combine(path, "noimage.jpeg")),
          Characteristics = new ItemCharacteristics
          {
            Description = "item1 desc",
            Color = "red"
          }
        },
        new Item
        {
          Title = "item2",
          Price = 10,
          CountsInStock = 20,
          Image = await File.ReadAllBytesAsync(Path.Combine(path, "noimage.jpeg")),
          Characteristics = new ItemCharacteristics
          {
            Description = "item2 desc",
            Color = "blue"
          }
        },
        new Item
        {
          Title = "item3",
          Price = 5,
          CountsInStock = 50,
          Image = await File.ReadAllBytesAsync(Path.Combine(path, "noimage.jpeg")),
          Characteristics = new ItemCharacteristics
          {
            Description = "item3 desc",
            Model = "Minsk"
          }
        },
        new Item
        {
          Title = "item4",
          Price = 35,
          CountsInStock = 8,
          Image = await File.ReadAllBytesAsync(Path.Combine(path, "noimage.jpeg")),
          Characteristics = new ItemCharacteristics
          {
            Description = "item4 desc",
            Size = "L"
          }
        },
        new Item
        {
          Title = "item5",
          Price = 35,
          CountsInStock = 8,
          Image = await File.ReadAllBytesAsync(Path.Combine(path, "noimage.jpeg")),
          Characteristics = new ItemCharacteristics
          {
            Description = "item5 desc",
            Size = "M"
          }
        }
      };
    }
  }
}
