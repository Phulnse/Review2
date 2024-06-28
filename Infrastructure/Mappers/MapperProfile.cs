using Application.ViewModels;
using Application.ViewModels.CategoryVMs;
using Application.ViewModels.ContractTypeVMs;
using Application.ViewModels.ContractVMs;
using Application.ViewModels.CouncilVMs;
using Application.ViewModels.DepartmentVMs;
using Application.ViewModels.DocumentVMs;
using Application.ViewModels.FileTypeVMs;
using Application.ViewModels.HolidayVMs;
using Application.ViewModels.NationVMs;
using Application.ViewModels.NotifyVMs;
using Application.ViewModels.ProvinceVMs;
using Application.ViewModels.RemunerationVMs;
using Application.ViewModels.ReviewVMs;
using Application.ViewModels.TopicVMs;
using Application.ViewModels.UserVMs;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.Mappers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CreateTopicReq, Topic>();

            CreateMap<Category, GetAllCategoriesRes>().ForMember(x => x.CategoryId, opt => opt.MapFrom(src => src.Id));

            CreateMap<User, UserInforRes>().ForMember(x => x.DepartmentName, opt => opt.MapFrom(src => src.Department.DepartmentName));

            CreateMap<Topic, TopicDetailRes>().ForMember(x => x.TopicId, opt => opt.MapFrom(src => src.Id))
                                            .ForMember(x => x.TopicLeaderName, opt => opt.MapFrom(src => src.Creator.FullName))
                                            .ForMember(x => x.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName));

            CreateMap<Council, CouncilInforRes>().ForMember(x => x.CouncilId, opt => opt.MapFrom(src => src.Id))
                                                    .ForMember(x => x.Email, opt => opt.MapFrom(src => src.User.AccountEmail))
                                                    .ForMember(x => x.Name, opt => opt.MapFrom(src => src.User.FullName))
                                                    .ForMember(x => x.PhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber));

            CreateMap<Topic, TopicInfoRes>().ForMember(x => x.TopicId, opt => opt.MapFrom(src => src.Id))
                                            .ForMember(x => x.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName));

            CreateMap<AddMemberToTopic, Participant>();

            CreateMap<DeanMakeDecisionReq, Topic>().ForMember(x => x.Id, opt => opt.Ignore())
                                                    .ForMember(x => x.DeciderId, opt => opt.MapFrom(src => src.DiciderId));

            CreateMap<Topic, GetTopicDecidedByDeanIdRes>().ForMember(x => x.TopicId, opt => opt.MapFrom(src => src.Id))
                                                            .ForMember(x => x.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName));

            CreateMap<Topic, TopicInforForReviewerRes>().ForMember(x => x.TopicId, opt => opt.MapFrom(src => src.Id))
                                                        .ForMember(x => x.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName));

            CreateMap<ConfigEarlyReviewReq, Review>();

            CreateMap<ConfigMiddleReviewReq, Review>();

            CreateMap<ConfigFinalReviewReq, Review>();

            CreateMap<AddCouncil, Council>().ForMember(x => x.UserId, opt => opt.MapFrom(src => src.CouncilId));

            CreateMap<Topic, TopicDetailRes>().ForMember(x => x.TopicId, opt => opt.MapFrom(src => src.Id))
                                            .ForMember(x => x.TopicLeaderName, opt => opt.MapFrom(src => src.Creator.FullName))
                                            .ForMember(x => x.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                                            .ForMember(x => x.NumberOfMember, opt => opt.MapFrom(src => src.Participants.Count + 1));

            CreateMap<Topic, TopicInforForUser>().ForMember(x => x.TopicId, opt => opt.MapFrom(src => src.Id))
                                                .ForMember(x => x.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName));

            CreateMap<Topic, TopicDocumentsRes>().ForMember(x => x.TopicId, opt => opt.MapFrom(src => src.Id))
                                                .ForMember(x => x.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                                                .ForMember(x => x.ReviewEarly, opt => opt.MapFrom(src => src.Reviews.Where(x => x.State == ReviewStateEnum.EarlyTermReport).FirstOrDefault()));

            CreateMap<Review, ReviewEarly>().ForMember(x => x.ReviewId, opt => opt.MapFrom(src => src.Id));

            CreateMap<Document, DocumentOfReview>().ForMember(x => x.DocumentId, opt => opt.MapFrom(src => src.Id));

            CreateMap<NewFile, Contract>().ForMember(x => x.ContractName, opt => opt.MapFrom(src => src.FileName))
                                            .ForMember(x => x.ContractLink, opt => opt.MapFrom(src => src.FileLink));

            CreateMap<Topic, TopicProcessRes>().ForMember(x => x.TopicId, opt => opt.MapFrom(src => src.Id));

            CreateMap<Topic, EarlyTopicForCouncilRes>().ForMember(x => x.TopicId, opt => opt.MapFrom(src => src.Id))
                                                        .ForMember(x => x.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                                                        .ForMember(x => x.Deadline, opt => opt.MapFrom(src => src.Reviews.Where(x => x.State == ReviewStateEnum.EarlyTermReport).First().ResubmitDeadline))
                                                        .ForMember(x => x.Result, opt => opt.MapFrom(src => src.Reviews.Where(x => x.State == ReviewStateEnum.EarlyTermReport).First().DecisionOfCouncil.ToString()));

            CreateMap<Topic, TopicForCouncilRes>().ForMember(x => x.TopicId, opt => opt.MapFrom(src => src.Id))
                                                    .ForMember(x => x.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName));

            CreateMap<Topic, TopicWaitingResubmitRes>().ForMember(x => x.TopicId, opt => opt.MapFrom(src => src.Id))
                                                        .ForMember(x => x.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                                                        .ForMember(x => x.Deadline, opt => opt.MapFrom(src => src.Reviews.Where(x => x.State == ReviewStateEnum.EarlyTermReport).First().ResubmitDeadline));

            CreateMap<Topic, TopicReviewedForMemberRes>().ForMember(x => x.TopicId, opt => opt.MapFrom(src => src.Id))
                                                            .ForMember(x => x.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName));

            CreateMap<Review, ReviewMeetingInfor>().ForMember(x => x.ReviewId, opt => opt.MapFrom(src => src.Id));

            CreateMap<Review, ReviewEarlyDocument>();

            CreateMap<Document, DocumentOfEarlyReview>();

            CreateMap<MakeMiddleReviewScheduleReq, Review>();

            CreateMap<MakeFinalReviewScheduleReq, Review>();

            CreateMap<Topic, TopicWaitingUploadMeetingMinutesRes>().ForMember(x => x.TopicId, opt => opt.MapFrom(src => src.Id))
                                                                    .ForMember(x => x.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName));

            CreateMap<UploadEarlyContractReq, Contract>();

            CreateMap<UploadContractForEndingPhaseReq, Contract>();

            CreateMap<ContractType, GetContractTypeRes>();

            CreateMap<FileType, GetFileTypeRes>();

            CreateMap<Department, GetDepartmentReq>().ForMember(x => x.DepartmentId, opt => opt.MapFrom(src => src.Id));

            CreateMap<SubmitRemunerationReq, Remuneration>();

            CreateMap<Remuneration, GetRemunerationRes>();

            CreateMap<Holiday, GetHolidaysRes>();

            CreateMap<Province, GetProvinceVM>();

            CreateMap<Nation, GetNationVM>();

            CreateMap<Notify, NotifyVM>().ForMember(x => x.TopicName, opt => opt.MapFrom(src => src.Topic.TopicName));

            CreateMap<UserVM, User>();
        }
    }
}
