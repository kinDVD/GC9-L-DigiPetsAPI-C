using System;
using System.Collections.Generic;

namespace PetAPI.Models;

public partial class DigiPet
{
    public int Id { get; set; }

    public int? AccountId { get; set; }

    public string? Name { get; set; }

    public decimal? Health { get; set; }

    public int? Strength { get; set; }

    public int? Experience { get; set; }
}
