using BusinessAccess.Repository.IRepository;
using DataAccess.DTOs;
using System.Reflection;

namespace BusinessAccess.Repository
{
    public class PaginationRepository<T, U> : IPaginationRepository<T, U> where T : class where U : class
    {
        public PageFilterResponseDTO<T> GetPagedData(List<T> entity, PageFilterRequestDTO<U> pageFilterDTO)
        {

            PageFilterResponseDTO<T> pageFilterResponseDTO = new()
            {
                TotalColumn = typeof(T).GetProperties().Length,
                CurrentPage = pageFilterDTO.currentpage,
                PageSize = pageFilterDTO.pagesize,
                OrderColumnName = pageFilterDTO.OrderByColumnName,
                OrderBy = pageFilterDTO.OrderBy
            };

            PropertyInfo[] searchProperties = typeof(T).GetProperties();

            /// searching from key
            string? searchValue = pageFilterDTO.search?.ToLower();

            List<T> list = new();
            if (!string.IsNullOrEmpty(searchValue))
            {
                foreach (var entityProperty in searchProperties)
                {
                    if (entityProperty.Name == "Id")
                        continue;
                    list = list.Union(entity.Where(e => (bool)(entityProperty.GetValue(e)?.ToString().ToLower().Contains(searchValue)))
                                    .OrderBy(e => entityProperty.GetValue(e)?.ToString().IndexOf(searchValue) ?? int.MaxValue)
                                    .ThenBy(e => entityProperty.GetValue(e)?.ToString())
                                    .ToList()).ToList();
                }
            }
            else
            {
                list = entity;
            }
            entity = list;




            /// Searching from filter
            var searchColumns = pageFilterDTO.SearchByColumns;

            if (searchColumns != null)
            {
                var columnProperties = searchColumns.GetType().GetProperties();

                foreach (var columnProperty in columnProperties)
                {
                    var thissearchValue = columnProperty.GetValue(searchColumns)?.ToString().ToLower();
                    if (!string.IsNullOrEmpty(thissearchValue))
                    {
                        var entityProperty = searchProperties.FirstOrDefault(p => p.Name == columnProperty.Name);
                        if (entityProperty != null)
                        {
                            entity = entity
                                .Where(e => (bool)(entityProperty.GetValue(e)?.ToString().ToLower().Contains(thissearchValue)))
                                .OrderBy(e => entityProperty.GetValue(e)?.ToString().IndexOf(thissearchValue) ?? int.MaxValue)
                                .ThenBy(e => entityProperty.GetValue(e)?.ToString())
                                .ToList();
                        }
                    }
                }
            }

            /// Info Of Pages
            var pages = (decimal)entity.Count / (decimal)pageFilterDTO.pagesize;
            pageFilterResponseDTO.TotalPage = (int)Math.Ceiling(pages);
            pageFilterResponseDTO.TotalRecords = entity.Count;

            /// Sorting
            if (pageFilterDTO.OrderByColumnName != null)
            {
                var propertyInfo = typeof(T).GetProperty(pageFilterDTO.OrderByColumnName);

                if (propertyInfo != null)
                {
                    entity = pageFilterDTO.OrderBy
                        ? entity.OrderBy(e => propertyInfo.GetValue(e, null)).ToList()
                        : entity.OrderByDescending(e => propertyInfo.GetValue(e, null)).ToList();
                }
            }

            /// Paging
            entity = entity.Skip((pageFilterDTO.currentpage - 1) * pageFilterDTO.pagesize).Take(pageFilterDTO.pagesize).ToList();

            ///Response
            pageFilterResponseDTO.Data = entity;
            return pageFilterResponseDTO;
        }
    }
}
