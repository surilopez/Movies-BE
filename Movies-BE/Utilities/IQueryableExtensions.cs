using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Movies_BE.DTOs;

namespace Movies_BE.Utilities
{
    public  static class IQueryableExtensions
    {
        public static IQueryable<T> Pagin<T>(this IQueryable<T> queryable, PaginationDTO paginationDTO) {
            return queryable
                    .Skip((paginationDTO.Page - 1) * paginationDTO.recordsByPage)
                    .Take(paginationDTO.recordsByPage);
        }
    }
}
