import { createContext, useContext, useState, ReactNode, FC } from "react";
import { ServerResponse } from "types/ServerResponse";
import { AccountSet, AccountSetAppend } from "types/AccountSet";
import { handleApiError } from "api/handleApiError";
import axios from "axios";

interface AccountSetContextProps {
    accounts: AccountSet[];
    account: AccountSet | null;
    getMainAccounts: () => Promise<ServerResponse<AccountSet[]>>;
    getSubAccounts: () => Promise<ServerResponse<AccountSet[]>>;
    getMainAccountById: (id: number) => Promise<ServerResponse<AccountSet>>;
    getSubAccountById: (id: number) => Promise<ServerResponse<AccountSet>>;
    createMainAccount: (
        account: AccountSetAppend
    ) => Promise<ServerResponse<null>>;
    createSubAccount: (
        account: AccountSetAppend
    ) => Promise<ServerResponse<null>>;
    updateAccount: (account: AccountSet) => Promise<ServerResponse<null>>;
    deleteAccount: (account: AccountSet) => Promise<ServerResponse<null>>;
}

const AccountSetContext = createContext<AccountSetContextProps | undefined>(
    undefined
);

export const AccountSetProvider: FC<{ children: ReactNode }> = ({
    children,
}) => {
    const [accounts, setAccounts] = useState<AccountSet[]>([]);
    const [account, setAccount] = useState<AccountSet | null>(null);

    const getMainAccounts = async (): Promise<ServerResponse<AccountSet[]>> => {
        try {
            const res = await axios.get<ServerResponse<AccountSet[]>>(
                "AccountSets/index_main"
            );
            setAccounts(res.data.data);
            return res.data;
        } catch (error) {
            return handleApiError<AccountSet[]>(error);
        }
    };

    const getSubAccounts = async (): Promise<ServerResponse<AccountSet[]>> => {
        try {
            const res = await axios.get<ServerResponse<AccountSet[]>>(
                "AccountSets/index_sub"
            );
            setAccounts(res.data.data);
            return res.data;
        } catch (error) {
            return handleApiError<AccountSet[]>(error);
        }
    };

    const getMainAccountById = async (
        id: number
    ): Promise<ServerResponse<AccountSet>> => {
        try {
            const res = await axios.get<ServerResponse<AccountSet>>(
                `AccountSets/details_main/${id}`
            );
            setAccount(res.data.data);
            return res.data;
        } catch (error) {
            return handleApiError<AccountSet>(error);
        }
    };

    const getSubAccountById = async (
        id: number
    ): Promise<ServerResponse<AccountSet>> => {
        try {
            const res = await axios.get<ServerResponse<AccountSet>>(
                `AccountSets/details_sub/${id}`
            );
            setAccount(res.data.data);
            return res.data;
        } catch (error) {
            return handleApiError<AccountSet>(error);
        }
    };

    const createMainAccount = async (
        account: AccountSetAppend
    ): Promise<ServerResponse<null>> => {
        try {
            const res = await axios.post<ServerResponse<null>>(
                `AccountSets/create_main`,
                account
            );
            return res.data;
        } catch (error) {
            return handleApiError<null>(error);
        }
    };

    const createSubAccount = async (
        account: AccountSetAppend
    ): Promise<ServerResponse<null>> => {
        try {
            const res = await axios.post<ServerResponse<null>>(
                `AccountSets/create_sub`,
                account
            );
            return res.data;
        } catch (error) {
            return handleApiError<null>(error);
        }
    };

    const updateAccount = async (
        account: AccountSet
    ): Promise<ServerResponse<null>> => {
        try {
            const res = await axios.put<ServerResponse<null>>(
                `AccountSets/edit`,
                account
            );
            return res.data;
        } catch (error) {
            return handleApiError<null>(error);
        }
    };

    const deleteAccount = async (
        account: AccountSet
    ): Promise<ServerResponse<null>> => {
        try {
            const res = await axios.delete<ServerResponse<null>>(
                `AccountSets/delete`,
                { data: account }
            );
            return res.data;
        } catch (error) {
            return handleApiError<null>(error);
        }
    };

    return (
        <AccountSetContext.Provider
            value={{
                accounts,
                account,
                getMainAccounts,
                getSubAccounts,
                getMainAccountById,
                getSubAccountById,
                createMainAccount,
                createSubAccount,
                updateAccount,
                deleteAccount,
            }}
        >
            {children}
        </AccountSetContext.Provider>
    );
};

export const useAccountSetContext = () => {
    const context = useContext(AccountSetContext);

    if (!context) {
        throw new Error(
            "useAccountSetContext must be used within a AccountSetProvider"
        );
    }
    return context;
};
