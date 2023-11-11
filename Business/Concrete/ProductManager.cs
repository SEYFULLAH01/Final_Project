﻿using Business.Absract;
using Business.CCS;
using Business.Constants;
using Business.ValidationRules;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Results;
using DataAccess.Absract;
using DataAccess.Concrete.InMemory;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        IProductDal _productDal;

        public ProductManager(IProductDal productDal)
        {

            _productDal = productDal;

        }
        [ValidationAspect(typeof(ProductValidator))]
        public IResult Add(Product product)
        {
            // business codes 
            if(CheckIfCategoryCountOfCategoryCorrect(product.CategoryId).Success);
            {
                if (CheckIfProductNameExists(product.ProductName).Success)
                {
                    _productDal.Add(product);

                    return (IResult)new SuccessResult(Messages.ProductAdded);
                }          
            }
            return (IResult) new ErrorResult();
        }

        public IDataResult<List<Product>> GetAll()
        {
            if (DateTime.Now.Hour ==2)
            {
                return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
            }
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(), Messages.ProductsListed);
        }

        public IDataResult<List<Product>> GetAllByCategoryId(int id)
        {
            return  new SuccessDataResult<List<Product>>(_productDal.GetAll(p=>p.CategoryId == id));
        }

        public IDataResult<Product> GetById(int productId)
        {
            return new SuccessDataResult<Product>(_productDal.Get(p => p.ProductId == productId));
        }

        public IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.UnitPrice>=min && p.UnitPrice<=max));
        }

        public IDataResult<List<ProductDetailDto>> GetProductDetails()
        {
            return new SuccessDataResult < List < ProductDetailDto >> (_productDal.GetProductDetails());
        }

        [ValidationAspect(typeof(ProductValidator))]
        public IResult Uptade(Product product)
        {
            var result = _productDal.GetAll(P => P.CategoryId == product.CategoryId).Count;
            if (result >= 10)
            {
                return (IResult)new ErrorResult(Messages.ProductCountOfCategoryError);
            }
            throw new NotImplementedException();
        }

        private IResult CheckIfCategoryCountOfCategoryCorrect(int categoryId)
        {
            var result = _productDal.GetAll(P => P.CategoryId == categoryId).Count;
            if (result >= 15)
            {
                return (IResult)new ErrorResult(Messages.ProductCountOfCategoryError);
            }
            return  (IResult) new SuccessResult();
        }
        private IResult CheckIfProductNameExists(string productName)
        {
            var result = _productDal.GetAll(P => P.ProductName == productName).Any();
            if (result)
            {
                return (IResult)new ErrorResult(Messages.ProductNameAlreadyExists);
            }
            return (IResult)new SuccessResult();
        }
    }
}