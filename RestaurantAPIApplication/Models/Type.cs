using System;
using System.Collections.Generic;

namespace RestaurantAPIApplication.Models;

public partial class Type
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Place> Places { get; set; } = new List<Place>();
}
