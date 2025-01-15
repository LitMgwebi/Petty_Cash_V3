using Backend.Models;
using Backend.Services.DocumentService.DeleteDocument;
using Backend.Services.DocumentService.GetAllDocuments;
using Backend.Services.DocumentService.GetOneDocument;
using Backend.Services.DocumentService.UploadDocument;
using Backend.Services.RequisitionService;
using System.Security.Claims;

namespace Backend.Services.DocumentService
{
    public class DocumentService(BackendContext db, IRequisition requisition, IHttpContextAccessor httpContextAccessor) : IDocument
    {
        private BackendContext _db = db;
        private IRequisition _requisition = requisition;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        private string GetUserName() => _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Name)!;`       
       
        public async Task<ServerResponse<IEnumerable<Document>>> GetAllDocuments(string command, int requisitionId)
        {
            try
            {
                ServerResponse<IEnumerable<Document>> response = new();
                DocumentIndexer indexer = new DocumentIndexer();

                if (command == "motivation")
                {
                    if (requisitionId == 0)
                    {
                        indexer.setState(new GetMotivationsState(_db));
                        response = await indexer.request();
                    }
                    else if (requisitionId > 0)
                    {
                        indexer.setState(new GetMotivationsByRequisitionState(_db, requisitionId));
                        response = await indexer.request();
                    }
                }
                else if (command == "receipt")
                {
                    if (requisitionId == 0)
                    {
                        indexer.setState(new GetReceiptsState(_db));
                        response = await indexer.request();
                    }
                    else if (requisitionId > 0)
                    {
                        indexer.setState(new GetReceiptsByRequisitionState(_db, requisitionId));
                        response = await indexer.request();
                    }
                }
                else
                {
                    return new ServerResponse<IEnumerable<Document>>
                    {
                        Success = false,
                        Message = "System could not resolve document command when fetching documents."
                    };
                }

                return response;
            }
            catch (Exception ex)
            {
                return new ServerResponse<IEnumerable<Document>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServerResponse<Document>> GetOneDocument(string command, int documentId)
        {
            try
            {
                ServerResponse<Document> response = new();
                DocumentDetailer detailer = new DocumentDetailer();
                if (command == "motivations")
                {
                    detailer.setState(new GetOneMotivationState(_db, documentId));
                    response = await detailer.GetDocument();
                }
                else if (command == "receipts")
                {
                    detailer.setState(new GetOneReceiptState(_db, documentId));
                    response = await detailer.GetDocument();
                }
                else
                {
                    return new ServerResponse<Document>
                    {
                        Success = false,
                        Message = $"System could not retrieve document with id #{documentId}."
                    };
                }

                return response;
            }
            catch (Exception ex)
            {
                return new ServerResponse<Document>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServerResponse<Document>> UploadDocument(string command, IFormFile file, int requisitionId)
        {
            string name = GetUserName();
            try
            {
                ServerResponse<Document> response = new();
                DocumentUploader uploader = new DocumentUploader();

                if (file == null)
                    return new ServerResponse<Document>
                    {
                        Success = false,
                        Message = "File was not found."
                    };

                string[] allowedFileTypes = { "pdf" };
                string fileExtension = Path.GetExtension(file.FileName).Substring(1);

                if (!allowedFileTypes.Contains(fileExtension))
                {
                    return new ServerResponse<Document>
                    {
                        Success = false,
                        Message = $"File format {Path.GetExtension(file.FileName)} is invalid for this operation. Please select a PDF file."
                    };
                }
                if (file.Length > 0)
                {
                    if (file.Length < 5242880)
                    {
                        if (command == Document.Motivation)
                        {
                            uploader.setState(new UploadMotivationState(_db, _requisition, file, name, requisitionId, fileExtension));
                            response = await uploader.request();
                        }
                        else if (command == Document.Receipt)
                        {
                            uploader.setState(new UploadReceiptState(_db, _requisition, file, name, requisitionId, fileExtension));
                            response = await uploader.request();
                        }
                        else
                            return new ServerResponse<Document>
                            {
                                Success = false,
                                Message = "Could not recognize command given for uploading document."
                            };
                    }
                    else
                    {
                        return new ServerResponse<Document>
                        {
                            Success = false,
                            Message = "The document you uploaded is too large, please constrict size to 5 MB"
                        };
                    }
                }
                else
                {
                    return new ServerResponse<Document>
                    {
                        Success = false,
                        Message = "Cannot load file."
                    };
                }

                return response;
            }
            catch (Exception ex)
            {
                return new ServerResponse<Document>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServerResponse<Document>> EditDocument(Document document)
        {
            try
            {
                _db.Documents.Update(document);
                if (await _db.SaveChangesAsync() == 0)
                    return new ServerResponse<Document>
                    {
                        Success = false,
                        Message = $"System could not edit ${document.FileName}.{document.FileExtension}."
                    };

                return new ServerResponse<Document>
                {
                    Success = true,
                    Message = "Document edited."
                };
            }
            catch (Exception ex)
            {
                return new ServerResponse<Document>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServerResponse<Document>> DeleteDocument(Document document, string typeOfDocument)
        {
            try
            {
                ServerResponse<Document> response = new();
                DocumentDeleter deleter = new DocumentDeleter();

                if (typeOfDocument == Document.Motivation)
                {
                    deleter.setState(new DeleteMotivationState());
                    response = await deleter.request(_db, _requisition, document);
                }
                else if (typeOfDocument == Document.Receipt)
                {
                    deleter.setState(new DeleteReceiptState());
                    response = await deleter.request(_db, _requisition, document);
                }
                else
                    return new ServerResponse<Document>
                    {
                        Success = false,
                        Message = "Could not recognize command given for uploading document."
                    };

                return response;
            }
            catch (Exception ex)
            {
                return new ServerResponse<Document>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

    }
}
