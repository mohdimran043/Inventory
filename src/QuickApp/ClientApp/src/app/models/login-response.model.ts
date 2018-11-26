// ====================================================

// Email: support@ebenmonney.com
// ====================================================

import { PermissionValues } from './permission.model';


export interface LoginResponse {
  access_token: string;
  id_token: string;
  refresh_token: string;
  expires_in: number;
}


export interface IdToken {
  sub: string;
  id: string;
  userName: string;
  empDisplayName: string;
  mNo: string;
  empDeptCode: string;
  empDeptName: string;
  empRank: string;
  empRankCode: string;
  empProf: string;
  empProfCode: string;
  role: string | string[];
  permission: PermissionValues | PermissionValues[];
  configuration: string;
  isEnabled: string;
}
