using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace DataAccess.CustomClasses
{
    public class PaginatedList<T> : List<T>
    {
        public int CurrentPage      { get; private set; } /*شماره صفحه فعلی*/
        public int CountSizePerPage { get; private set; } /*تعداد داده های قابل نمایش برای هر صفحه*/
        public int TotalPages       { get; private set; } /*تعداد کل صفحات بر اساس تعداد ردیف های Entity مورد نظر و با در نظر گرفتن CountSizePerPage*/

        /*----------------------------------------------------------*/
        
        public bool HasPrev => CurrentPage > 1;          /*در این قسمت بررسی می شود که آیا لینک صفحه قبلی فعال باشد یا خیر*/
        public bool HasNext => CurrentPage < TotalPages; /*در این قسمت بررسی می شود که آیا لینک صفحه بعدی فعال باشد یا خیر*/

        /*----------------------------------------------------------*/
        
        public PaginatedList(List<T> Data, int CountRowData, int CountSizePerPage, int CurrentPageNumber)
        {
            this.CountSizePerPage = CountSizePerPage;
            this.CurrentPage      = CurrentPageNumber;
            this.TotalPages       = (int)Math.Ceiling(CountRowData / (double) CountSizePerPage);
            
            base.AddRange(Data);
        }
    }
}