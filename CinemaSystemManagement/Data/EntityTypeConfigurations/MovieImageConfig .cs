using CinemaSystemManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaSystemManagement.Data.EntityTypeConfigurations
{
    public class MovieImageConfig : IEntityTypeConfiguration<MovieImage>
    {
        public void Configure(EntityTypeBuilder<MovieImage> builder)
        {
            builder.Property(i => i.ImageUrl)
                   .IsRequired();

            builder.HasOne(i => i.Movie)
                   .WithMany(m => m.SubImages)
                   .HasForeignKey(i => i.MovieId);
        }
    }
}
