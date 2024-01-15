﻿using AutoMapper;
using EstateAgentAPI.Buisness.DTO;
using EstateAgentAPI.Business.DTO;
using EstateAgentAPI.Business.Services;
using EstateAgentAPI.Persistence.Models;
using EstateAgentAPI.Persistence.Repositories;
using System;

namespace EstateAgentAPI.Business.Services
{
    public class SellerService : ISellerService
    {


        ISellerRepository _sellerRepository;
        private IMapper _mapper;

        public SellerService(ISellerRepository repository, IMapper mapper)
        {
            _sellerRepository = repository;
            _mapper = mapper;
        }

        public SellerDTO Create(SellerDTO dtoSeller)
        {
            Seller sellerData = _mapper.Map<Seller>(dtoSeller);
            sellerData = _sellerRepository.Create(sellerData);
            dtoSeller = _mapper.Map<SellerDTO>(sellerData);
            return dtoSeller;
        }

        public void Delete(SellerDTO dtoSeller)
        {
            Seller seller = _mapper.Map<Seller>(dtoSeller);
            _sellerRepository.Delete(seller);
        }

        public IQueryable<SellerDTO> FindAll()
        {
            var sellers = _sellerRepository.FindAll().ToList();
            List<SellerDTO> dtoSeller = new List<SellerDTO>();
            foreach (Seller seller in sellers)
            {
                dtoSeller.Add(_mapper.Map<SellerDTO>(seller));
            }
            return dtoSeller.AsQueryable();
        }

        public SellerDTO FindById(int id)
        {
            Seller seller = _sellerRepository.FindById(id);
            SellerDTO dtoSeller = _mapper.Map<SellerDTO>(seller);
            return dtoSeller;
        }

        public SellerDTO Update(SellerDTO dtoSeller)
        {
            Seller sellerData = _mapper.Map<Seller>(dtoSeller);
            var sel = _sellerRepository.FindById(sellerData.Id);
            if (sel == null)
                return null;

            sel.FirstName = sellerData.FirstName;
            sel.Surname = sellerData.Surname;
            sel.Address = sellerData.Address;
            sel.Postcode = sellerData.Postcode;
            sel.Phone = sellerData.Phone;

            Seller seller = _sellerRepository.Update(sel);
            SellerDTO dtoSel = _mapper.Map<SellerDTO>(seller);
            return dtoSel;
        }
    }
}
