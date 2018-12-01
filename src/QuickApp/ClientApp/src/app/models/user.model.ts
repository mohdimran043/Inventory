// ====================================================

// Email: support@ebenmonney.com
// ====================================================

export class User {
  // Note: Using only optional constructor properties without backing store disables typescript's type checking for the type
  constructor(id?: string, userName?: string, empDisplayName?: string, mNo?: string, empDeptCode?: string, empDeptName?: string,
    empRank?: string, empRankCode?: string, empProf?: string, empProfCode?: string, roles?: string[]) {

    this.id = id;
    this.userName = userName;
    this.empDisplayName = empDisplayName;
    this.mNo = mNo;
    this.empDeptCode = empDeptCode;
    this.empDeptName = empDeptName;
    this.empRank = empRank;
    this.empRankCode = empRankCode;
    this.empProf = empProf;
    this.empProfCode = empProfCode;
    this.roles = roles;
  }

  public id: string;
  public userName: string;
  public empDisplayName: string;
  public mNo: string;
  public empDeptCode: string;
  public empDeptName: string;
  public empRank: string;
  public empRankCode: string;
  public empProf: string;
  public empProfCode: string;
  public isEnabled: boolean;
  public roles: string[];
  
}
export enum UserType {
  admin = 1,
  maintainance = 2,
  operations = 3,
  dispather = 4
}
