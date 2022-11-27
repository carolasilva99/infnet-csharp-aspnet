using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GerenciadorAniversarios.Models;

namespace GerenciadorAniversarios.Data
{
    public class GerenciadorAniversariosContext : DbContext
    {
        public GerenciadorAniversariosContext (DbContextOptions<GerenciadorAniversariosContext> options)
            : base(options)
        {
        }

        public DbSet<GerenciadorAniversarios.Models.Pessoa> Pessoa { get; set; } = default!;
    }
}
