using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Service.DTO
{
    public class ArtistDto
    {
        public Guid Id { get; set; } = Guid.Empty;

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name must be 50 characters at maximum")]
        public string Name { get; set; }

        [StringLength(1000, ErrorMessage = "Description must be 1000 characters at maximum")]
        public string Description { get; set; }

        [Url(ErrorMessage = "Facebook Link must be a correct URL")]
        public string FacebookLink { get; set; }

        public List<Guid> ImageIds { get; set; }
    }
}
