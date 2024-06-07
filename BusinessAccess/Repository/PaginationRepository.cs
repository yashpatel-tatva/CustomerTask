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
            PropertyInfo[] SearchProperties = typeof(T).GetProperties();

            /// Searching from key
            string? SearchValue = pageFilterDTO.Search?.ToLower();
            List<T> list = new();
            if (!string.IsNullOrEmpty(SearchValue))
            {
                foreach (PropertyInfo entityProperty in SearchProperties)
                {
                    if (entityProperty.Name == "Id")
                        continue;
                    list = list.Union(entity.Where(e => (entityProperty.GetValue(e)?.ToString().ToLower().Contains(SearchValue) ?? false))
                                    .OrderBy(e => entityProperty.GetValue(e)?.ToString().IndexOf(SearchValue) ?? int.MaxValue)
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
            U SearchColumns = pageFilterDTO.SearchByColumns;

            if (SearchColumns != null)
            {
                PropertyInfo[] columnProperties = SearchColumns.GetType().GetProperties();

                foreach (PropertyInfo columnProperty in columnProperties)
                {
                    string? thisSearchValue = columnProperty.GetValue(SearchColumns)?.ToString().ToLower();
                    if (!string.IsNullOrEmpty(thisSearchValue))
                    {
                        PropertyInfo entityProperty = SearchProperties.FirstOrDefault(p => p.Name == columnProperty.Name);
                        if (entityProperty != null)
                        {
                            entity = entity
                                .Where(e => (bool)(entityProperty.GetValue(e)?.ToString().ToLower().Contains(thisSearchValue)))
                                .OrderBy(e => entityProperty.GetValue(e)?.ToString().IndexOf(thisSearchValue) ?? int.MaxValue)
                                .ThenBy(e => entityProperty.GetValue(e)?.ToString())
                                .ToList();
                        }
                    }
                }
            }

            /// Info Of Pages
            decimal pages = (decimal)entity.Count / (decimal)pageFilterDTO.pagesize;
            pageFilterResponseDTO.TotalPage = (int)Math.Ceiling(pages);
            pageFilterResponseDTO.TotalRecords = entity.Count;

            /// Sorting
            if (pageFilterDTO.OrderByColumnName != null)
            {
                PropertyInfo propertyInfo = typeof(T).GetProperty(pageFilterDTO.OrderByColumnName);

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
