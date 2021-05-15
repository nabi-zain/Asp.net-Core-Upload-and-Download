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

        [HttpGet]
        public async Task<IActionResult> Download(int DocumentRepositoryID, int customerID)
        {
            var documents = await CustomerFleetManagementService.ViewDocumentAsync(DocumentRepositoryID);
            FileInfo fileExtension = new FileInfo(documents.ImgName);
            string path = @"C:\Temp2";
            System.IO.Directory.CreateDirectory(path);
            FileStream stream = new FileStream(Path.Combine(path, documents.ImgName), FileMode.Create);
            stream.Write(documents.ImgData, 0, documents.ImgLength);
            stream.Close();
            new ProcessStartInfo(@"C:\Temp2\"+documents.ImgName).StartProcess();
            return RedirectToAction("Files", "Customer", new { customerID = customerID });
        }
