import { QSortSet } from "../../feature/QSort/QSortSet";

export interface ApiResponse {
  status: number;
  data: any;
}

export interface ApiCall {
  (): Promise<ApiResponse>;
}

export interface APIs {
  CalcEditDistance: ApiCall;
  GetSampleIntegerArray: (
    maxNumberValue: number,
    arrayItemCount: number
  ) => Promise<{ status: number; data: number[] | null }>;
  SortIntegerArray: (
    arrayToSort: number[]
  ) => Promise<{ status: number; data: QSortSet | null }>;
}

const API = (): APIs => {
  const apiUrl: string = process.env.API_URL ?? "";

  const apiCall = async <T>(
    url: string,
    method: string,
    body: any = undefined
  ): Promise<{ status: number; data: T | null }> => {
    const response = await fetch(url, {
      method: method,
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json",
      },
      body: body ? JSON.stringify(body) : undefined,
    });

    if (response.ok) {
      return {
        status: response.status,
        data: (await response.json()) as T,
      };
    }
    return {
      status: response.status,
      data: null,
    };
  };

  const CalcEditDistance = async () => {
    return { status: 500, data: apiUrl };
  };
  const GetSampleIntegerArray = async (
    maxNumberValue: number,
    arrayItemCount: number
  ) => {
    return await apiCall<number[]>(
      `${process.env.PUBLIC_URL}/api/qsort/getrandomarray?maxValue=${maxNumberValue}&arraySize=${arrayItemCount}`,
      "GET"
    );
  };
  const SortIntegerArray = async (arrayToSort: number[]) => {
    return await apiCall<QSortSet>(
      `${process.env.PUBLIC_URL}/api/qsort/pivotarray`,
      "POST",
      arrayToSort
    );
  };

  return {
    CalcEditDistance,
    GetSampleIntegerArray,
    SortIntegerArray,
  };
};

export default API;
