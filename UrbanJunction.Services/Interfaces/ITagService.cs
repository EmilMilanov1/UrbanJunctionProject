using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanJunction.Data.Models;
public interface ITagService
{
    Task<Tag> GetOrCreateAsync(string name);
    Task<IEnumerable<Tag>> GetAllAsync();
}