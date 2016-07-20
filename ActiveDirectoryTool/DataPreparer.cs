﻿using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Dynamic;
using System.Threading;
using Extensions.PrincipalExtensions;
using ActiveDirectoryPropertyMapping = System.Collections.Generic.Dictionary
    <ActiveDirectoryTool.ActiveDirectoryProperty, System.Action>;
using static ActiveDirectoryTool.ActiveDirectoryProperty;

namespace ActiveDirectoryTool
{
    internal class DataPreparer
    {
        private readonly CancellationToken _cancellationToken;

        internal DataPreparer(CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
        }

        internal dynamic PrepareData(
            IEnumerable<ActiveDirectoryProperty> properties,
            ComputerPrincipal computerPrincipal = null,
            GroupPrincipal groupPrincipal = null,
            GroupPrincipal containerGroupPrincipal = null,
            UserPrincipal directReportUserPrincipal = null,
            UserPrincipal managerUserPrincipal = null,
            UserPrincipal userPrincipal = null)
        {
            dynamic result = new ExpandoObject();
            foreach (var property in properties)
            {
                _cancellationToken.ThrowIfCancellationRequested();
                if (AddComputerProperty(
                    computerPrincipal, property, result))
                {
                    continue;
                }

                if (AddContainerGroupProperty(
                    containerGroupPrincipal, property, result))
                {
                    continue;
                }

                if (AddDirectReportProperty(
                    directReportUserPrincipal, property, result))
                {
                    continue;
                }

                if (AddGroupProperty(
                    groupPrincipal, property, result))
                {
                    continue;
                }

                AddUserProperty(userPrincipal, property, result);
            }

            return result;
        }

        private static bool AddComputerProperty(
            ComputerPrincipal computerPrincipal,
            ActiveDirectoryProperty property,
            dynamic result)
        {
            var propertyMapping = new ActiveDirectoryPropertyMapping
            {
                [ComputerAccountExpirationDate] = () =>
                {
                    result.ComputerAccountExpirationDate =
                        computerPrincipal?.AccountExpirationDate;
                },
                [ComputerAccountLockoutTime] = () =>
                {
                    result.ComputerAccountLockoutTime =
                        computerPrincipal?.AccountLockoutTime;
                },
                [ComputerAllowReversiblePasswordEncryption] = () =>
                {
                    result.ComputerAllowReversiblePasswordEncryption =
                        computerPrincipal?
                            .AllowReversiblePasswordEncryption;
                },
                [ComputerBadLogonCount] = () =>
                {
                    result.ComputerBadLogonCount =
                        computerPrincipal?.BadLogonCount;
                },
                [ComputerCertificates] = () =>
                {
                    result.ComputerCertificates =
                        computerPrincipal?.Certificates;
                },
                [ComputerContext] = () =>
                {
                    result.ComputerContext = computerPrincipal?.Context;
                },
                [ComputerContextType] = () =>
                {
                    result.ComputerContextType =
                        computerPrincipal?.ContextType;
                },
                [ComputerDelegationPermitted] = () =>
                {
                    result.ComputerDelegationPermitted =
                        computerPrincipal?.DelegationPermitted;
                },
                [ComputerDescription] = () =>
                {
                    result.ComputerDescription =
                        computerPrincipal?.Description;
                },
                [ComputerDisplayName] = () =>
                {
                    result.ComputerDisplayName =
                        computerPrincipal?.DisplayName;
                },
                [ComputerDistinguishedName] = () =>
                {
                    result.ComputerDistinguishedName =
                        computerPrincipal?.DistinguishedName;
                },
                [ComputerEnabled] = () =>
                {
                    result.ComputerEnabled = computerPrincipal?.Enabled;
                },
                [ComputerGuid] = () =>
                {
                    result.ComputerGuid = computerPrincipal?.Guid;
                },
                [ComputerHomeDirectory] = () =>
                {
                    result.ComputerHomeDirectory =
                        computerPrincipal?.HomeDirectory;
                },
                [ComputerHomeDrive] = () =>
                {
                    result.ComputerHomeDrive =
                        computerPrincipal?.HomeDrive;
                },
                [ComputerLastBadPasswordAttempt] = () =>
                {
                    result.ComputerLastBadPasswordAttempt =
                        computerPrincipal?.LastBadPasswordAttempt;
                },
                [ComputerLastLogon] = () =>
                {
                    result.ComputerLastLogon =
                        computerPrincipal?.LastLogon;
                },
                [ComputerLastPasswordSet] = () =>
                {
                    result.LastPasswordSet =
                        computerPrincipal?.LastPasswordSet;
                },
                [ComputerName] = () =>
                {
                    result.ComputerName = computerPrincipal?.Name;
                },
                [ComputerPasswordNeverExpires] = () =>
                {
                    result.ComputerPasswordNeverExpires =
                        computerPrincipal?.PasswordNeverExpires;
                },
                [ComputerPasswordNotRequired] = () =>
                {
                    result.ComputerPasswordNotRequired =
                        computerPrincipal?.PasswordNotRequired;
                },
                [ComputerPermittedLogonTimes] = () =>
                {
                    result.ComputerPermittedLogonTimes =
                        computerPrincipal?.PermittedLogonTimes;
                },
                [ComputerPermittedWorkstations] = () =>
                {
                    result.ComputerPermittedWorkstations =
                        computerPrincipal?.PermittedWorkstations;
                },
                [ComputerSamAccountName] = () =>
                {
                    result.ComputerSamAccountName =
                        computerPrincipal?.SamAccountName;
                },
                [ComputerScriptPath] = () =>
                {
                    result.ComputerScriptPath =
                        computerPrincipal?.ScriptPath;
                },
                [ComputerServicePrincipalNames] = () =>
                {
                    result.ComputerServicecomputerPrincipalNames =
                        computerPrincipal?.ServicePrincipalNames;
                },
                [ComputerSid] = () =>
                {
                    result.ComputerSid = computerPrincipal?.Sid;
                },
                [ComputerSmartcardLogonRequired] = () =>
                {
                    result.ComputerSmartcardLogonRequired =
                        computerPrincipal?.SmartcardLogonRequired;
                },
                [ComputerStructuralObjectClass] = () =>
                {
                    result.ComputerStructuralObjectClass =
                        computerPrincipal?.StructuralObjectClass;
                },
                [ComputerUserCannotChangePassword] = () =>
                {
                    result.ComputerUserCannotChangePassword =
                        computerPrincipal?.UserCannotChangePassword;
                },
                [ComputerUserPrincipalName] = () =>
                {
                    result.ComputerUsercomputerPrincipalName =
                        computerPrincipal?.UserPrincipalName;
                }
            };

            if (!propertyMapping.ContainsKey(property)) return false;
            propertyMapping[property]();
            return true;
        }

        private static bool AddContainerGroupProperty(
            GroupPrincipal containerGroupPrincipal,
            ActiveDirectoryProperty property,
            dynamic result)
        {
            var propertyMapping = new ActiveDirectoryPropertyMapping
            {
                [ContainerGroupContext] = () =>
                {
                    result.ContainerGroupContext =
                        containerGroupPrincipal?.Context;
                },
                [ContainerGroupContextType] = () =>
                {
                    result.ContainerGroupContextType =
                        containerGroupPrincipal?.ContextType;
                },
                [ContainerGroupDescription] = () =>
                {
                    result.ContainerGroupDescription =
                        containerGroupPrincipal?.Description;
                },
                [ContainerGroupDisplayName] = () =>
                {
                    result.ContainerGroupDisplayName =
                        containerGroupPrincipal?.DisplayName;
                },
                [ContainerGroupDistinguishedName] = () =>
                {
                    result.ContainerGroupDistinguishedName =
                        containerGroupPrincipal?.DistinguishedName;
                },
                [ContainerGroupGuid] = () =>
                {
                    result.ContainerGroupGuid = containerGroupPrincipal?.Guid;
                },
                [ContainerGroupIsSecurityGroup] = () =>
                {
                    result.ContainerGroupIsSecurityGroup =
                        containerGroupPrincipal?.IsSecurityGroup;
                },
                [ContainerGroupManagedBy] = () =>
                {
                    result.ContainerGroupManagedBy =
                        containerGroupPrincipal?.GetManagedBy();
                },
                [ContainerGroupName] = () =>
                {
                    result.ContainerGroupName = containerGroupPrincipal?.Name;
                },
                [ContainerGroupSamAccountName] = () =>
                {
                    result.ContainerGroupSamAccountName =
                        containerGroupPrincipal?.SamAccountName;
                },
                [ContainerGroupScope] = () =>
                {
                    result.ContainerGroupScope =
                        containerGroupPrincipal?.GroupScope;
                },
                [ContainerGroupSid] = () =>
                {
                    result.ContainerGroupSid = containerGroupPrincipal?.Sid;
                },
                [ContainerGroupStructuralObjectClass] = () =>
                {
                    result.ContainerGroupStructuralObjectClass =
                        containerGroupPrincipal?.StructuralObjectClass;
                },
                [ContainerGroupUserPrincipalName] = () =>
                {
                    result.ContainerGroupUserPrincipalName =
                        containerGroupPrincipal?.UserPrincipalName;
                },
                [ContainerGroupMembers] = () =>
                {
                    result.ContainerGroupMembers =
                        containerGroupPrincipal?.Members;
                }
            };

            if (!propertyMapping.ContainsKey(property)) return false;
            propertyMapping[property]();
            return true;
        }

        private static bool AddDirectReportProperty(
            UserPrincipal directReportUserPrincipal,
            ActiveDirectoryProperty property,
            dynamic result)
        {
            var propertyMapping = new ActiveDirectoryPropertyMapping
            {
                [DirectReportUserAccountControl] = () =>
                {
                    result.DirectReportAccountControl =
                        directReportUserPrincipal.GetUserAccountControl();
                },
                [DirectReportAccountExpirationDate] = () =>
                {
                    result.DirectReportAccountExpirationDate =
                        directReportUserPrincipal.AccountExpirationDate;
                },
                [DirectReportAccountLockoutTime] = () =>
                {
                    result.DirectReportAccountLockoutTime =
                        directReportUserPrincipal.AccountLockoutTime;
                },
                [DirectReportAllowReversiblePasswordEncryption] = () =>
                {
                    result.DirectReportAllowReversiblePasswordEncryption =
                        directReportUserPrincipal
                            .AllowReversiblePasswordEncryption;
                },
                [DirectReportAssistant] = () =>
                {
                    result.DirectReportAssistant =
                        directReportUserPrincipal.GetAssistant();
                },
                [DirectReportBadLogonCount] = () =>
                {
                    result.DirectReportBadLogonCount =
                        directReportUserPrincipal.BadLogonCount;
                },
                [DirectReportCertificates] = () =>
                {
                    result.DirectReportCertificates =
                        directReportUserPrincipal.Certificates;
                },
                [DirectReportCity] = () =>
                {
                    result.DirectReportCity =
                        directReportUserPrincipal.GetCity();
                },
                [DirectReportComment] = () =>
                {
                    result.DirectReportComment =
                        directReportUserPrincipal.GetComment();
                },
                [DirectReportCompany] = () =>
                {
                    result.DirectReportCompany =
                        directReportUserPrincipal.GetCompany();
                },
                [DirectReportContext] = () =>
                {
                    result.DirectReportContext =
                        directReportUserPrincipal.Context;
                },
                [DirectReportContextType] = () =>
                {
                    result.DirectReportContextType =
                        directReportUserPrincipal.ContextType;
                },
                [DirectReportCountry] = () =>
                {
                    result.DirectReportCountry =
                        directReportUserPrincipal.GetCountry();
                },
                [DirectReportDelegationPermitted] = () =>
                {
                    result.DirectReportDelegationPermitted =
                        directReportUserPrincipal.DelegationPermitted;
                },
                [DirectReportDepartment] = () =>
                {
                    result.DirectReportDepartment =
                        directReportUserPrincipal.GetDepartment();
                },
                [DirectReportDescription] = () =>
                {
                    result.DirectReportDescription =
                        directReportUserPrincipal.Description;
                },
                [DirectReportDisplayName] = () =>
                {
                    result.DirectReportDisplayName =
                        directReportUserPrincipal.DisplayName;
                },
                [DirectReportDistinguishedName] = () =>
                {
                    result.DirectReportDistinguishedName =
                        directReportUserPrincipal?.DistinguishedName;
                },
                [DirectReportDivision] = () =>
                {
                    result.DirectReportDivision =
                        directReportUserPrincipal.GetDivision();
                },
                [DirectReportEmailAddress] = () =>
                {
                    result.DirectReportEmailAddress =
                        directReportUserPrincipal.EmailAddress;
                },
                [DirectReportEmployeeId] = () =>
                {
                    result.DirectReportEmployeeId =
                        directReportUserPrincipal.EmployeeId;
                },
                [DirectReportEnabled] = () =>
                {
                    result.DirectReportEnabled =
                        directReportUserPrincipal.Enabled;
                },
                [DirectReportFax] = () =>
                {
                    result.DirectReportFax =
                        directReportUserPrincipal.GetFax();
                },
                [DirectReportSuffix] = () =>
                {
                    result.DirectReportSuffix =
                        directReportUserPrincipal.GetSuffix();
                },
                [DirectReportGivenName] = () =>
                {
                    result.DirectReportGivenName =
                        directReportUserPrincipal.GivenName;
                },
                [DirectReportGuid] = () =>
                {
                    result.DirectReportGuid =
                        directReportUserPrincipal.Guid;
                },
                [DirectReportHomeAddress] = () =>
                {
                    result.DirectReportHomeAddress =
                        directReportUserPrincipal.GetHomeAddress();
                },
                [DirectReportHomeDirectory] = () =>
                {
                    result.DirectReportHomeDirectory =
                        directReportUserPrincipal.HomeDirectory;
                },
                [DirectReportHomeDrive] = () =>
                {
                    result.DirectReportHomeDrive =
                        directReportUserPrincipal.HomeDrive;
                },
                [DirectReportHomePhone] = () =>
                {
                    result.DirectReportHomePhone =
                        directReportUserPrincipal.GetHomePhone();
                },
                [DirectReportInitials] = () =>
                {
                    result.DirectReportInitials =
                        directReportUserPrincipal.GetInitials();
                },
                [DirectReportIsAccountLockedOut] = () =>
                {
                    result.DirectReportIsAccountLockedOut =
                        directReportUserPrincipal.IsAccountLockedOut();
                },
                [DirectReportIsActive] = () =>
                {
                    result.DirectReportIsActive =
                        directReportUserPrincipal.IsActive();
                },
                [DirectReportLastBadPasswordAttempt] = () =>
                {
                    result.DirectReportLastBadPasswordAttempt =
                        directReportUserPrincipal.LastBadPasswordAttempt;
                },
                [DirectReportLastLogon] = () =>
                {
                    result.DirectReportLastLogon =
                        directReportUserPrincipal.LastLogon;
                },
                [DirectReportLastPasswordSet] = () =>
                {
                    result.DirectReportLastPasswordSet =
                        directReportUserPrincipal.LastPasswordSet;
                },
                [DirectReportManager] = () =>
                {
                    result.DirectReportManager =
                        directReportUserPrincipal.GetManager();
                },
                [DirectReportMiddleName] = () =>
                {
                    result.DirectReportMiddleName =
                        directReportUserPrincipal.MiddleName;
                },
                [DirectReportMobile] = () =>
                {
                    result.DirectReportMobile =
                        directReportUserPrincipal.GetMobile();
                },
                [DirectReportName] = () =>
                {
                    result.DirectReportName =
                        directReportUserPrincipal?.Name;
                },
                [DirectReportNotes] = () =>
                {
                    result.DirectReportNotes =
                        directReportUserPrincipal.GetNotes();
                },
                [DirectReportPager] = () =>
                {
                    result.DirectReportPager =
                        directReportUserPrincipal.GetPager();
                },
                [DirectReportPasswordNeverExpires] = () =>
                {
                    result.DirectReportPasswordNeverExpires =
                        directReportUserPrincipal.PasswordNeverExpires;
                },
                [DirectReportPasswordNotRequired] = () =>
                {
                    result.DirectReportPasswordNotRequired =
                        directReportUserPrincipal.PasswordNotRequired;
                },
                [DirectReportPermittedLogonTimes] = () =>
                {
                    result.DirectReportPermittedLogonTimes =
                        directReportUserPrincipal.PermittedLogonTimes;
                },
                [DirectReportPermittedWorkstations] = () =>
                {
                    result.DirectReportPermittedWorkstations =
                        directReportUserPrincipal.PermittedWorkstations;
                },
                [DirectReportSamAccountName] = () =>
                {
                    result.DirectReportSamAccountName =
                        directReportUserPrincipal.SamAccountName;
                },
                [DirectReportScriptPath] = () =>
                {
                    result.DirectReportScriptPath =
                        directReportUserPrincipal.ScriptPath;
                },
                [DirectReportSid] = () =>
                {
                    result.DirectReportSid = directReportUserPrincipal.Sid;
                },
                [DirectReportSip] = () =>
                {
                    result.DirectReportSip =
                        directReportUserPrincipal.GetSip();
                },
                [DirectReportSmartcardLogonRequired] = () =>
                {
                    result.DirectReportSmartcardLogonRequired =
                        directReportUserPrincipal.SmartcardLogonRequired;
                },
                [DirectReportState] = () =>
                {
                    result.DirectReportState =
                        directReportUserPrincipal.GetState();
                },
                [DirectReportStreetAddress] = () =>
                {
                    result.DirectReportStreetAddress =
                        directReportUserPrincipal.GetStreetAddress();
                },
                [DirectReportStructuralObjectClass] = () =>
                {
                    result.DirectReportStructuralObjectClass =
                        directReportUserPrincipal.StructuralObjectClass;
                },
                [DirectReportSurname] = () =>
                {
                    result.DirectReportSurname =
                        directReportUserPrincipal.Surname;
                },
                [DirectReportTitle] = () =>
                {
                    result.DirectReportTitle =
                        directReportUserPrincipal.GetTitle();
                },
                [DirectReportUserCannotChangePassword] = () =>
                {
                    result.DirectReportUserCannotChangePassword =
                        directReportUserPrincipal.UserCannotChangePassword;
                },
                [DirectReportUserPrincipalName] = () =>
                {
                    result.DirectReportdirectReportUserPrincipalName =
                        directReportUserPrincipal.UserPrincipalName;
                },
                [DirectReportVoiceTelephoneNumber] = () =>
                {
                    result.DirectReportVoiceTelephoneNumber =
                        directReportUserPrincipal.VoiceTelephoneNumber;
                },
                [DirectReportVoip] = () =>
                {
                    result.DirectReportVoip =
                        directReportUserPrincipal.GetVoip();
                }
            };

            if (!propertyMapping.ContainsKey(property)) return false;
            propertyMapping[property]();
            return true;
        }

        private static bool AddGroupProperty(
            GroupPrincipal principal,
            ActiveDirectoryProperty property,
            dynamic result)
        {
            var propertyMapping = new ActiveDirectoryPropertyMapping
            {
                [GroupContext] = () =>
                    {
                        result.GroupContext = principal.Context;
                    },
                [GroupContextType] = () =>
                {
                    result.GroupContextType = principal.ContextType;
                },
                [GroupDescription] = () =>
                {
                    result.GroupDescription = principal.Description;
                },
                [GroupDisplayName] = () =>
                {
                    result.GroupDisplayName = principal.DisplayName;
                },
                [GroupDistinguishedName] = () =>
                {
                    result.GroupDistinguishedName =
                        principal.DistinguishedName;
                },
                [GroupGuid] = () =>
                {
                    result.GroupGuid = principal.Guid;
                },
                [GroupIsSecurityGroup] = () =>
                {
                    result.GroupIsSecurityGroup =
                        principal.IsSecurityGroup;
                },
                [GroupManagedBy] = () =>
                {
                    result.GroupManagedBy = principal.GetManagedBy();
                },
                [GroupName] = () =>
                {
                    result.GroupName = principal.Name;
                },
                [GroupSamAccountName] = () =>
                {
                    result.GroupSamAccountName = principal.SamAccountName;
                },
                [ActiveDirectoryProperty.GroupScope] = () =>
                {
                    result.GroupScope = principal.GroupScope;
                },
                [GroupSid] = () =>
                {
                    result.GroupSid = principal.Sid;
                },
                [GroupStructuralObjectClass] = () =>
                {
                    result.GroupStructuralObjectClass =
                        principal.StructuralObjectClass;
                },
                [GroupUserPrincipalName] = () =>
                {
                    result.GroupUserPrincipalName =
                        principal.UserPrincipalName;
                },
                [GroupMembers] = () =>
                {
                    result.GroupMembers = principal.Members;
                }
            };

            if (!propertyMapping.ContainsKey(property)) return false;
            propertyMapping[property]();
            return true;
        }

        private static void AddUserProperty(
            UserPrincipal userPrincipal,
            ActiveDirectoryProperty property,
            dynamic result)
        {
            var propertyMapping = new ActiveDirectoryPropertyMapping
            {
                [UserUserAccountControl] = () =>
                {
                    result.UserAccountControl =
                        userPrincipal.GetUserAccountControl();
                },
                [UserAccountExpirationDate] = () =>
                {
                    result.UserAccountExpirationDate =
                        userPrincipal.AccountExpirationDate;
                },
                [UserAccountLockoutTime] = () =>
                {
                    result.UserAccountLockoutTime =
                        userPrincipal.AccountLockoutTime;
                },
                [UserAllowReversiblePasswordEncryption] = () =>
                {
                    result.UserAllowReversiblePasswordEncryption =
                        userPrincipal.AllowReversiblePasswordEncryption;
                },
                [UserAssistant] = () =>
                {
                    result.UserAssistant = userPrincipal.GetAssistant();
                },
                [UserBadLogonCount] = () =>
                {
                    result.UserBadLogonCount = userPrincipal.BadLogonCount;
                },
                [UserCertificates] = () =>
                {
                    result.UserCertificates = userPrincipal.Certificates;
                },
                [UserCity] = () =>
                {
                    result.UserCity = userPrincipal.GetCity();
                },
                [UserComment] = () =>
                {
                    result.UserComment = userPrincipal.GetComment();
                },
                [UserCompany] = () =>
                {
                    result.UserCompany = userPrincipal.GetCompany();
                },
                [UserContext] = () =>
                {
                    result.UserContext = userPrincipal.Context;
                },
                [UserContextType] = () =>
                {
                    result.UserContextType = userPrincipal.ContextType;
                },
                [UserCountry] = () =>
                {
                    result.UserCountry = userPrincipal.GetCountry();
                },
                [UserDelegationPermitted] = () =>
                {
                    result.UserDelegationPermitted =
                        userPrincipal.DelegationPermitted;
                },
                [UserDepartment] = () =>
                {
                    result.UserDepartment = userPrincipal.GetDepartment();
                },
                [UserDescription] = () =>
                {
                    result.UserDescription = userPrincipal.Description;
                },
                [UserDisplayName] = () =>
                {
                    result.UserDisplayName = userPrincipal.DisplayName;
                },
                [UserDistinguishedName] = () =>
                {
                    result.UserDistinguishedName =
                        userPrincipal.DistinguishedName;
                },
                [UserDivision] = () =>
                {
                    result.UserDivision = userPrincipal.GetDivision();
                },
                [UserEmailAddress] = () =>
                {
                    result.UserEmailAddress = userPrincipal.EmailAddress;
                },
                [UserEmployeeId] = () =>
                {
                    result.UserEmployeeId = userPrincipal.EmployeeId;
                },
                [UserEnabled] = () =>
                {
                    result.UserEnabled = userPrincipal.Enabled;
                },
                [UserFax] = () =>
                {
                    result.UserFax = userPrincipal.GetFax();
                },
                [UserSuffix] = () =>
                {
                    result.UserSuffix = userPrincipal.GetSuffix();
                },
                [UserGivenName] = () =>
                {
                    result.UserGivenName = userPrincipal.GivenName;
                },
                [UserGuid] = () =>
                {
                    result.UserGuid = userPrincipal.Guid;
                },
                [UserHomeAddress] = () =>
                {
                    result.UserHomeAddress = userPrincipal.GetHomeAddress();
                },
                [UserHomeDirectory] = () =>
                {
                    result.UserHomeDirectory = userPrincipal.HomeDirectory;
                },
                [UserHomeDrive] = () =>
                {
                    result.UserHomeDrive = userPrincipal.HomeDrive;
                },
                [UserHomePhone] = () =>
                {
                    result.UserHomePhone = userPrincipal.GetHomePhone();
                },
                [UserInitials] = () =>
                {
                    result.UserInitials = userPrincipal.GetInitials();
                },
                [UserIsAccountLockedOut] = () =>
                {
                    result.UserIsAccountLockedOut =
                        userPrincipal.IsAccountLockedOut();
                },
                [UserIsActive] = () =>
                {
                    result.UserIsActive = userPrincipal.IsActive();
                },
                [UserLastBadPasswordAttempt] = () =>
                {
                    result.UserLastBadPasswordAttempt =
                        userPrincipal.LastBadPasswordAttempt;
                },
                [UserLastLogon] = () =>
                {
                    result.UserLastLogon = userPrincipal.LastLogon;
                },
                [UserLastPasswordSet] = () =>
                {
                    result.UserLastPasswordSet = userPrincipal.LastPasswordSet;
                },
                [UserManager] = () =>
                {
                    result.UserManager = userPrincipal.GetManager();
                },
                [UserMiddleName] = () =>
                {
                    result.UserMiddleName = userPrincipal.MiddleName;
                },
                [UserMobile] = () =>
                {
                    result.UserMobile = userPrincipal.GetMobile();
                },
                [UserName] = () =>
                {
                    result.UserName = userPrincipal.Name;
                },
                [UserNotes] = () =>
                {
                    result.UserNotes = userPrincipal.GetNotes();
                },
                [UserPager] = () =>
                {
                    result.UserPager = userPrincipal.GetPager();
                },
                [UserPasswordNeverExpires] = () =>
                {
                    result.UserPasswordNeverExpires =
                        userPrincipal.PasswordNeverExpires;
                },
                [UserPasswordNotRequired] = () =>
                {
                    result.UserPasswordNotRequired =
                        userPrincipal.PasswordNotRequired;
                },
                [UserPermittedLogonTimes] = () =>
                {
                    result.UserPermittedLogonTimes =
                        userPrincipal.PermittedLogonTimes;
                },
                [UserPermittedWorkstations] = () =>
                {
                    result.UserPermittedWorkstations =
                        userPrincipal.PermittedWorkstations;
                },
                [UserSamAccountName] = () =>
                {
                    result.UserSamAccountName = userPrincipal.SamAccountName;
                },
                [UserScriptPath] = () =>
                {
                    result.UserScriptPath = userPrincipal.ScriptPath;
                },
                [UserSid] = () =>
                {
                    result.UserSid = userPrincipal.Sid;
                },
                [UserSip] = () =>
                {
                    result.UserSip = userPrincipal.GetSip();
                },
                [UserSmartcardLogonRequired] = () =>
                {
                    result.UserSmartcardLogonRequired =
                        userPrincipal.SmartcardLogonRequired;
                },
                [UserState] = () =>
                {
                    result.UserState = userPrincipal.GetState();
                },
                [UserStreetAddress] = () =>
                {
                    result.UserStreetAddress =
                        userPrincipal.GetStreetAddress();
                },
                [UserStructuralObjectClass] = () =>
                {
                    result.UserStructuralObjectClass =
                        userPrincipal.StructuralObjectClass;
                },
                [UserSurname] = () =>
                {
                    result.UserSurname = userPrincipal.Surname;
                },
                [UserTitle] = () =>
                {
                    result.UserTitle = userPrincipal.GetTitle();
                },
                [UserUserCannotChangePassword] = () =>
                {
                    result.UserUserCannotChangePassword =
                        userPrincipal.UserCannotChangePassword;
                },
                [UserUserPrincipalName] = () =>
                {
                    result.UserUserPrincipalName =
                        userPrincipal.UserPrincipalName;
                },
                [UserVoiceTelephoneNumber] = () =>
                {
                    result.UserVoiceTelephoneNumber =
                        userPrincipal.VoiceTelephoneNumber;
                },
                [UserVoip] = () =>
                {
                    result.UserVoip = userPrincipal.GetVoip();
                }
            };

            if (!propertyMapping.ContainsKey(property)) return;
            propertyMapping[property]();
        }
    }
}
