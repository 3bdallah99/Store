using AdminDashBoard.Models;
using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Store.DashBoard.Helpers;

namespace AdminDashBoard.Controllers
{
	public class ProductController(IUnitOfWork _unitOfWork , IMapper _mapper) : Controller
    {

        public async Task<IActionResult> Index()
	{
            var products = await _unitOfWork.GetRepository<Product,int>().GetAllAsync();
		var mappedProducts = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductViewModel>>((IReadOnlyList<Product>)products);
		return View(mappedProducts);
	}
	public IActionResult Create()
	{
		return View();
	}
	[HttpPost]
	public async Task<IActionResult> Create(ProductViewModel productViewModel)
	{
		if (ModelState.IsValid)
		{
			if (productViewModel.Image != null)
			{
				productViewModel.PictureUrl = PictureSettings.UploadFile(productViewModel.Image, "products");
			}
			else
				productViewModel.PictureUrl = "images/products/glazed-donuts.png";
			var mappedProduct = _mapper.Map<ProductViewModel, Product>(productViewModel);
			await _unitOfWork.GetRepository<Product,int>().AddAsync(mappedProduct);
			await _unitOfWork.SaveChangesAsync();
			return RedirectToAction("Index");
		}

		return View(productViewModel);
	}
	public async Task<IActionResult> Edit(int id)
	{
		var product = await _unitOfWork.GetRepository<Product,int >().GetAsync(id);
		var mappedProduct = _mapper.Map<Product, ProductViewModel>(product);
		return View(mappedProduct);
	}
	[HttpPost]
	[HttpPost]
	public async Task<IActionResult> Edit(int id, ProductViewModel productViewModel)
	{
		if (id != productViewModel.Id)
		{
			return NotFound();
		}

		if (ModelState.IsValid)
		{
			if (productViewModel.Image != null)
			{
				if (productViewModel.PictureUrl != null)
				{
					PictureSettings.DeleteFile(productViewModel.PictureUrl, "products");
				}

				productViewModel.PictureUrl = PictureSettings.UploadFile(productViewModel.Image, "products");
			}

			var mappedProduct = _mapper.Map<ProductViewModel, Product>(productViewModel);
			_unitOfWork.GetRepository<Product,int>().Update(mappedProduct);

			var result = await _unitOfWork.SaveChangesAsync();
			if (result > 0)
			{
				return RedirectToAction("Index");
			}
		}

		return View(productViewModel);
	}

	public async Task<IActionResult> Delete(int id)
	{
		var product = await _unitOfWork.GetRepository<Product,int>().GetAsync(id);
		var mappedProduct = _mapper.Map<Product, ProductViewModel>(product);
		return View(mappedProduct);
	}
	[HttpPost]
	public async Task<IActionResult> Delete(int id, ProductViewModel productViewModel)
	{
		if (id != productViewModel.Id)
			return NotFound();
		try
		{
			var product = await _unitOfWork.GetRepository<Product,int>().GetAsync(id);
			if (product.PictureUrl != null)
			{
				PictureSettings.DeleteFile(product.PictureUrl, "products");
			}
			_unitOfWork.GetRepository<Product,int>().Delete(product);
			await _unitOfWork.SaveChangesAsync();
			return RedirectToAction("Index");
		}
		catch (System.Exception)
		{
			return View(productViewModel);
		}
	}
}
}
