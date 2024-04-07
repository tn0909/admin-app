// models/company-response.model.ts

import { User } from "./user.model";

export class CompanyResponse {
  id?: string;
  name?: string;
  description?: string;
  address?: string;
  website?: string;
  users?: User[];
}
  