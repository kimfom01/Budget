using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Budget.Models;
using Budget.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Budget.Controllers;

public class HomeController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public HomeController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IActionResult> Index(int? id)
    {
        WalletCategoryTransactionViewModel viewModel;

        if (id == null)
        {
            var wallets = await _unitOfWork.Wallets.GetEntities(_ => true);

            viewModel = new WalletCategoryTransactionViewModel { Wallets = new SelectList(wallets, "Id", "Name") };
        }
        else
        {
            var wallets = await _unitOfWork.Wallets.GetEntities(_ => true);

            var wallet = await _unitOfWork.Wallets.GetEntity(id);

            var categories = await _unitOfWork.Categories.GetEntities(c => c.WalletId == wallet.Id);

            viewModel = new WalletCategoryTransactionViewModel
            {
                Wallets = new SelectList(wallets, "Id", "Name"), Wallet = wallet,
                CategoriesSelectList = new SelectList(categories, "CategoryId", "CategoryName")
            };
        }

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Index(int id, int categoryId)
    {
        var wallets = await _unitOfWork.Wallets.GetEntities(_ => true);

        var wallet = await _unitOfWork.Wallets.GetEntity(id);

        var categories = await _unitOfWork.Categories.GetEntities(c => c.WalletId == wallet.Id);

        var viewModel = new WalletCategoryTransactionViewModel
        {
            Wallets = new SelectList(wallets, "Id", "Name"), Wallet = wallet,
            CategoriesSelectList = new SelectList(categories, "CategoryId", "CategoryName")
        };

        // var transactions = _unitOfWork.Transactions.GetEntities(t => t.CategoryId == categories.Id);

        return View(viewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}