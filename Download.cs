        [HttpPost]
        public async Task<IActionResult> Files(List<IFormFile> File, int[] customerID, int categoryDocument)
        {           
            foreach (var file in File)
            {
                if(file.Length > 0)
                {
                    var document = new CustomerDocumentRepositoryModel
                    {
                        ImgName = file.FileName,
                        ImgContentType = file.ContentType,
                        ImgLength = Convert.ToInt32(file.Length),
                        CreatedByUserID = User.GetUserId(),
                        customerID = customerID[0],    
                        DocumentCategoryLCID = categoryDocument
                    };
                    var dataStream = new MemoryStream();
                    await file.CopyToAsync(dataStream);
                    document.ImgData = dataStream.ToArray();
                    await CustomerFleetManagementService.InsertDocument(document);
                }
            }

            return RedirectToAction("Files", "Customer", new { customerID = customerID[0] });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFiles(int documentRepositoryID, int customerID)
        {
            var customerDocumentRepository = new CustomerDocumentRepositoryModel();
            customerDocumentRepository.DocumentRepositoryID = documentRepositoryID;
            customerDocumentRepository.customerID = customerID;
            customerDocumentRepository.DeletedByUserID = User.GetUserId();
            await CustomerFleetManagementService.DeleteFile(customerDocumentRepository);

            return RedirectToAction("GetCustomersById", "Customer", new { customerID = customerID });
        }
