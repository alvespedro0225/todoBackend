using Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class TodoItemConfiguration : IEntityTypeConfiguration<TodoItem>
{
    public void Configure(EntityTypeBuilder<TodoItem> builder)
    {
        builder.HasKey(todo => todo.Id);

        builder.Property(todo => todo.Id)
            .ValueGeneratedNever();

        builder
            .HasOne(e => e.Owner)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade);
    }
}