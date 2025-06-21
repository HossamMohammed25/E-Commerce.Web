using Domain.Models.OrderModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data.Configurations
{
    internal class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            builder.ToTable("DeliveryMethods");
            builder.Property(D => D.Price).HasColumnType("decimal(8,2)");
            builder.Property(s => s.ShortName).HasColumnType("varchar(50)"); 
            builder.Property(s => s.DeliveryTime).HasColumnType("varchar(50)"); 
            builder.Property(s => s.Description).HasColumnType("varchar(100)"); 

        }
    }
}
