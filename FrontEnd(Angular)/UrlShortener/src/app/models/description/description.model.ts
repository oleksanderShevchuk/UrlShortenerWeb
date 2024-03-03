export interface Description {
    id: number;
    type: DescriptionType;
    content: string;
    lastUpdatedTime: Date;
  }
  
  export enum DescriptionType {
    ShorterAlgorithmDescription = 100
  }