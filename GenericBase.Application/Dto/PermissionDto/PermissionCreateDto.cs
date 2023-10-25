using GenericBase.Domain.Entities.Account;
using System.ComponentModel.DataAnnotations;

namespace GenericBase.Application.Dto.PermissionDto
{
    public class PermissionCreateDto
    {
        [Required, MinLength(5), MaxLength(40)]
        public string Key { get; set; } = string.Empty;

        [Required, MinLength(1), MaxLength(40)]
        public string Value { get; set; } = string.Empty;


        public static implicit operator Permission(PermissionCreateDto dto)
        {
            return new Permission(dto.Key, dto.Value, true);
        }

    }


}
