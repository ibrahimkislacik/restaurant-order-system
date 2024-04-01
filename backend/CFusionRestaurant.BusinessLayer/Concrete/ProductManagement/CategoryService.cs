using AutoMapper;
using CFusionRestaurant.BusinessLayer.Abstract.ProductManagement;
using CFusionRestaurant.DataLayer;
using CFusionRestaurant.Entities.ProductManagement;
using CFusionRestaurant.ViewModel.ExceptionManagement;
using CFusionRestaurant.ViewModel.ProductManagement;
using CFusionRestaurant.ViewModel.ProductManagement.Request;
using MongoDB.Bson;

namespace CFusionRestaurant.BusinessLayer.Concrete.ProductManagement
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(IRepository<Category> categoryRepository,
        IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<List<CategoryViewModel>> ListAsync()
        {
            var categories = await _categoryRepository.ListAsync().ConfigureAwait(false);
            return _mapper.Map<List<CategoryViewModel>>(categories);
        }

        public async Task<string> InsertAsync(CategoryInsertRequestViewModel categoryInsertViewModel)
        {
            var category = _mapper.Map<Category>(categoryInsertViewModel);
            category.CreatedDateTime = DateTime.Now;
            category.Id = ObjectId.GenerateNewId();
            await _categoryRepository.InsertAsync(category).ConfigureAwait(false);
            return category.Id.ToString();
        }

        public async Task<CategoryViewModel> GetAsync(string id)
        {
            var category = await _categoryRepository.GetAsync(id).ConfigureAwait(false);
            return _mapper.Map<CategoryViewModel>(category);
        }

        public async Task DeleteAsync(string id)
        {
            var category = await _categoryRepository.GetAsync(id).ConfigureAwait(false);
            if (category == null)
            {
                throw new NotFoundException($"Category with Id = {id} not found");
            }
            await _categoryRepository.DeleteAsync(id).ConfigureAwait(false);
        }

        public async Task UpdateAsync(CategoryUpdateRequestViewModel categoryUpdateViewModel)
        {
            var category = await _categoryRepository.GetAsync(categoryUpdateViewModel.Id).ConfigureAwait(false);
            if (category == null)
            {
                throw new NotFoundException($"Category with Id = {categoryUpdateViewModel.Id} not found");
            }
            category.Name = categoryUpdateViewModel.Name;
            await _categoryRepository.UpdateAsync(category).ConfigureAwait(false);
        }
    }
}
