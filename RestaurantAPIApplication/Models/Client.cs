using System;
using System.Collections.Generic;

namespace RestaurantAPIApplication.Models;

public partial class Client
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
}
