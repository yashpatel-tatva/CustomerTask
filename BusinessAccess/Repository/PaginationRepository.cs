using BusinessAccess.Repository.IRepository;
using CustomerTask;
using DataAccess.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace BusinessAccess.Repository
{
    public class PaginationRepository<T, U> : IPaginationRepository<T, U> where T : class where U : class
    {
        private readonly CustomerDbContext _customerDb;
        //private readonly ILogger _logger;
        public PaginationRepository(CustomerDbContext customerDb , ILogger logger)
        {
            _customerDb = customerDb;
            //_logger = logger;
        }
        //public PageFilterResponseDTO<T> GetPagedData(List<T> entity, PageFilterRequestDTO<U> pageFilterDTO)
        //{
        //    PageFilterResponseDTO<T> pageFilterResponseDTO = new()
        //    {
        //        TotalColumn = typeof(T).GetProperties().Length,
        //        CurrentPage = pageFilterDTO.currentpage,
        //        PageSize = pageFilterDTO.pagesize,
        //        OrderColumnName = pageFilterDTO.OrderByColumnName,
        //        OrderBy = pageFilterDTO.OrderBy
        //    };
        //    PropertyInfo[] SearchProperties = typeof(T).GetProperties();

        //    /// Searching from key
        //    string? SearchValue = pageFilterDTO.Search?.ToLower();
        //    List<T> list = new();
        //    if (!string.IsNullOrEmpty(SearchValue))
        //    {
        //        foreach (PropertyInfo entityProperty in SearchProperties)
        //        {
        //            if (entityProperty.Name == "Id")
        //                continue;
        //            list = list.Union(entity.Where(e => (entityProperty.GetValue(e)?.ToString().ToLower().Contains(SearchValue) ?? false))
        //                            .OrderBy(e => entityProperty.GetValue(e)?.ToString().IndexOf(SearchValue) ?? int.MaxValue)
        //                            .ThenBy(e => entityProperty.GetValue(e)?.ToString())
        //                            .ToList()).ToList();
        //        }
        //    }
        //    else
        //    {
        //        list = entity;
        //    }
        //    entity = list;

        //    /// Searching from filter
        //    U SearchColumns = pageFilterDTO.SearchByColumns;

        //    if (SearchColumns != null)
        //    {
        //        PropertyInfo[] columnProperties = SearchColumns.GetType().GetProperties();

        //        foreach (PropertyInfo columnProperty in columnProperties)
        //        {
        //            string? thisSearchValue = columnProperty.GetValue(SearchColumns)?.ToString().ToLower();
        //            if (!string.IsNullOrEmpty(thisSearchValue))
        //            {
        //                PropertyInfo entityProperty = SearchProperties.FirstOrDefault(p => p.Name == columnProperty.Name);
        //                if (entityProperty != null)
        //                {
        //                    entity = entity
        //                        .Where(e => (bool)(entityProperty.GetValue(e)?.ToString().ToLower().Contains(thisSearchValue)))
        //                        .OrderBy(e => entityProperty.GetValue(e)?.ToString().IndexOf(thisSearchValue) ?? int.MaxValue)
        //                        .ThenBy(e => entityProperty.GetValue(e)?.ToString())
        //                        .ToList();
        //                }
        //            }
        //        }
        //    }

        //    /// Info Of Pages
        //    decimal pages = (decimal)entity.Count / (decimal)pageFilterDTO.pagesize;
        //    pageFilterResponseDTO.TotalPage = (int)Math.Ceiling(pages);
        //    pageFilterResponseDTO.TotalRecords = entity.Count;

        //    /// Sorting
        //    if (pageFilterDTO.OrderByColumnName != null)
        //    {
        //        PropertyInfo propertyInfo = typeof(T).GetProperty(pageFilterDTO.OrderByColumnName);

        //        if (propertyInfo != null)
        //        {
        //            entity = pageFilterDTO.OrderBy
        //                ? entity.OrderBy(e => propertyInfo.GetValue(e, null)).ToList()
        //                : entity.OrderByDescending(e => propertyInfo.GetValue(e, null)).ToList();
        //        }
        //    }

        //    /// Paging
        //    entity = entity.Skip((pageFilterDTO.currentpage - 1) * pageFilterDTO.pagesize).Take(pageFilterDTO.pagesize).ToList();

        //    ///Response
        //    pageFilterResponseDTO.Data = entity;
        //    return pageFilterResponseDTO;
        //}

        public PageFilterResponseDTO<T> GetPagedDataTable(PageFilterRequestDTO<U> pageFilterDTO)
        {
            var query = _customerDb.Set<T>().AsQueryable();

            // Filter out entities where Isdelete is true
            var isDeleteProperty = typeof(T).GetProperty("Isdelete");
            if (isDeleteProperty != null)
            {
                var parameter = Expression.Parameter(typeof(T), "e");
                var propertyAccess = Expression.MakeMemberAccess(parameter, isDeleteProperty);
                var filterCondition = Expression.Equal(
                    propertyAccess,
                    Expression.Constant(false, typeof(bool?)) // Handle nullable boolean
                );
                var lambda = Expression.Lambda<Func<T, bool>>(filterCondition, parameter);

                query = query.Where(lambda);
            }

            // Filter out collection properties
            var properties = typeof(T).GetProperties()
               .Where(p => p.PropertyType == typeof(string));

            U SearchColumns = pageFilterDTO.SearchByColumns;

            // Searching from key
            string? SearchValue = pageFilterDTO.Search;
            var columnProperties = SearchColumns.GetType().GetProperties();
            if (!string.IsNullOrEmpty(SearchValue))
            {
                SearchValue = SearchValue.ToLower();
                var predicate = LinqKit.PredicateBuilder.New<T>(false); 

                foreach (var columnProperty in columnProperties)
                {
                    predicate = predicate.Or(e => EF.Functions.Like(EF.Property<string>(e, columnProperty.Name).ToLower(), $"%{SearchValue}%"));
                }

                query = query.Where(predicate);
            }

            //_logger.LogError("HERE SEE" + query.ToString());

            // Searching from filter
            if (SearchColumns != null)
            {
                foreach (var columnProperty in columnProperties)
                {
                    string? thisSearchValue = columnProperty.GetValue(SearchColumns)?.ToString();
                    if (!string.IsNullOrEmpty(thisSearchValue))
                    {
                        thisSearchValue = thisSearchValue.ToLower();
                        var entityProperty = typeof(T).GetProperty(columnProperty.Name);
                        if (entityProperty != null)
                        {
                            query = query.Where(e => EF.Functions.Like(EF.Property<string>(e, entityProperty.Name).ToLower(), $"%{thisSearchValue}%"));
                        }
                    }
                }
            }

            // Sorting
            if (!string.IsNullOrEmpty(pageFilterDTO.OrderByColumnName))
            {
                var propertyInfo = properties.FirstOrDefault(p => p.Name == pageFilterDTO.OrderByColumnName);
                if (propertyInfo != null)
                {
                    var parameter = Expression.Parameter(typeof(T), "e");
                    var propertyAccess = Expression.MakeMemberAccess(parameter, propertyInfo);
                    var lambda = Expression.Lambda<Func<T, object>>(Expression.Convert(propertyAccess, typeof(object)), parameter);

                    query = pageFilterDTO.OrderBy
                        ? query.OrderBy(lambda)
                        : query.OrderByDescending(lambda);
                }
            }

            // Total records count before pagination
            int totalRecords = query.Count();

            // Paging
            query = query.Skip((pageFilterDTO.currentpage - 1) * pageFilterDTO.pagesize).Take(pageFilterDTO.pagesize);

            // Data
            var data = query.ToList();

            // Response
            return new PageFilterResponseDTO<T>
            {
                Data = data,
                TotalRecords = totalRecords,
                TotalColumn = properties.Count(),
                CurrentPage = pageFilterDTO.currentpage,
                PageSize = pageFilterDTO.pagesize,
                TotalPage = (int)Math.Ceiling((decimal)totalRecords / pageFilterDTO.pagesize),
                OrderColumnName = pageFilterDTO.OrderByColumnName,
                OrderBy = pageFilterDTO.OrderBy
            };
        }

    }
}
