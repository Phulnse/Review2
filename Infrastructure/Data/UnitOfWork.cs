using Domain.Interfaces;
using Domain.Interfaces.IRepositories;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbContextTransaction _transaction;
        private readonly SRMSContext _context;
        public ITopicRepository _topicRepository;
        public IMemberReviewRepository _memberReviewRepository;
        public IParticipantRepository _userTopicRepository;
        public ICategoryRepository _categoryRepository;
        public IUserRepository _userRepository;
        public IReviewRepository _reviewRepository;
        public ICouncilRepository _councilRepository;
        public IStaffRepository _staffRepository;
        public IDocumentRepository _documentRepository;
        public IContractRepository _contractRepository;
        public IAccountRepository _accountRepository;
        public IContractTypeRepository _contractTypeRepository;
        public IFileTypeRepository _fileTypeRepository;
        public IRemunerationRepository _remunerationRepository;
        public IDepartmentRepository _departmentRepository { get; set; }
        public IHolidayRepository _holidayRepository { get; set; }
        public IProvinceRepository _provinceRepository { get; set; }
        public INationRepository _nationalRepository { get; set; }
        public INotifyRepository _notifyRepository { get; set; }

        public UnitOfWork(SRMSContext context)
        {
            _context = context;
        }

        public async Task BeginTransaction()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task Commit()
        {
            try
            {
                await _transaction.CommitAsync();
            }
            catch
            {
                await _transaction.RollbackAsync();
                throw;
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null!;
            }
        }

        public async Task Rollback()
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null!;
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool disposed = false;

        public ITopicRepository Topic
        {
            get
            {
                if (_topicRepository == null)
                {
                    _topicRepository = new TopicRepository(_context);
                }

                return _topicRepository;
            }
        }

        public IMemberReviewRepository MemberReview
        {
            get
            {
                if (_memberReviewRepository == null)
                {
                    _memberReviewRepository = new MemberReviewRepository(_context);
                }

                return _memberReviewRepository;
            }
        }

        public IParticipantRepository Participant
        {
            get
            {
                if (_userTopicRepository == null)
                {
                    _userTopicRepository = new ParticipantRepository(_context);
                }

                return _userTopicRepository;
            }
        }

        public ICategoryRepository Category
        {
            get
            {
                if (_categoryRepository == null)
                {
                    _categoryRepository = new CategoryRepository(_context);
                }

                return _categoryRepository;
            }
        }

        public IUserRepository User
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new UserRepository(_context);
                }

                return _userRepository;
            }
        }

        public IReviewRepository Review
        {
            get
            {
                if (_reviewRepository == null)
                {
                    _reviewRepository = new ReviewRepository(_context);
                }

                return _reviewRepository;
            }
        }

        public ICouncilRepository Council
        {
            get
            {
                if (_councilRepository == null)
                {
                    _councilRepository = new CouncilRepository(_context);
                }

                return _councilRepository;
            }
        }

        public IStaffRepository Staff
        {
            get
            {
                if (_staffRepository == null)
                {
                    _staffRepository = new StaffRepository(_context);
                }

                return _staffRepository;
            }
        }

        public IDocumentRepository Document
        {
            get
            {
                if (_documentRepository == null)
                {
                    _documentRepository = new DocumentRepository(_context);
                }

                return _documentRepository;
            }
        }

        public IContractRepository Contract
        {
            get
            {
                if (_contractRepository == null)
                {
                    _contractRepository = new ContractRepository(_context);
                }

                return _contractRepository;
            }
        }

        public IAccountRepository Account
        {
            get
            {
                if (_accountRepository == null)
                {
                    _accountRepository = new AccountRepository(_context);
                }

                return _accountRepository;
            }
        }

        public IContractTypeRepository ContractType
        {
            get
            {
                if (_contractTypeRepository == null)
                {
                    _contractTypeRepository = new ContractTypeRepository(_context);
                }

                return _contractTypeRepository;
            }
        }

        public IFileTypeRepository FileType
        {
            get
            {
                if (_fileTypeRepository == null)
                {
                    _fileTypeRepository = new FileTypeRepository(_context);
                }

                return _fileTypeRepository;
            }
        }

        public IDepartmentRepository Department
        {
            get
            {
                if (_departmentRepository == null)
                {
                    _departmentRepository = new DepartmentRepository(_context);
                }

                return _departmentRepository;
            }
        }

        public IRemunerationRepository Remuneration
        {
            get
            {
                if (_remunerationRepository == null)
                {
                    _remunerationRepository = new RemunerationRepository(_context);
                }

                return _remunerationRepository;
            }
        }

        public IHolidayRepository Holiday
        {
            get
            {
                if (_holidayRepository == null)
                {
                    _holidayRepository = new HolidayRepository(_context);
                }

                return _holidayRepository;
            }
        }

        public IProvinceRepository Province
        {
            get
            {
                if (_provinceRepository == null)
                {
                    _provinceRepository = new ProvinceRepository(_context);
                }

                return _provinceRepository;
            }
        }

        public INationRepository Nation
        {
            get
            {
                if (_nationalRepository == null)
                {
                    _nationalRepository = new NationRepository(_context);
                }

                return _nationalRepository;
            }
        }

        public INotifyRepository Notify
        {
            get
            {
                if (_notifyRepository == null)
                {
                    _notifyRepository = new NotifyRepository(_context);
                }

                return _notifyRepository;
            }
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }
    }
}
