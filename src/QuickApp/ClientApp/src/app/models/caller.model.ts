export class CallerInfo {
    Payload: string;
    Type: string;
    OrgId: string;
    CallerId: string;

    constructor(Payload: string = '', Type: string = '' , OrgId: string = '', CallerId: string = '' ) {
        this.Payload = Payload;
        this.Type = Type;
        this.OrgId = OrgId;
        this.CallerId = CallerId;
    }
  }
