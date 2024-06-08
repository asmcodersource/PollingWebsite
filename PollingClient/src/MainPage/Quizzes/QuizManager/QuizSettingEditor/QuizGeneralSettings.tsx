import './QuizGeneralSettings.css';
import AllowedUsersEditor from './AllowedUsersEditor/AllowedUsersEditor'
import Button from 'react-bootstrap/Button';
import { useMemo, useState } from 'react'

enum PollingType {
    Anyone,
    Authorized,
    OnlyAllowed,
    OnlyOwner
} 

interface QuizSettings {
    title: string,
    description: string,
    type: PollingType
};


const QuizGeneralSettings = (props: any) => {
    const [showAllowedUsers, setShowAllowedUsers] = useState<boolean>();
    const [settings, setSettings] = useState<QuizSettings>({ ...props.quiz, pollingType: props.quiz.type });

    const typesOfPolling = useMemo(() => {
        const values = Object.values(PollingType) as PollingType[];
        return values.slice(0, Math.floor(values.length / 2));
    }, [Object.values(PollingType)]);


    async function saveSettings() {
        try {
            let response = await fetch(`/api/polls/${props.quiz.id}`, {
                method: "PUT",
                headers: {
                    "Accept": "application/json",
                    "Content-Type": "application/json",
                    "Authorization": "Bearer " + sessionStorage.getItem('token')
                },
                body: JSON.stringify(
                    settings
                )
            });
        } catch ( exception: Error){
            console.log(exception);
        }
    }

    return (
        <div className="row quiz-manager-general-wrapper">
            <label>Title</label>
            <input
                type="text"
                value={settings?.title ?? ""}
                placeholder="Visible title of the survey"
                onChange={(e) => setSettings({ ...settings, title: e.target.value} as QuizSettings )}
            >
            </input><br />
            <label>Description</label>
            <textarea
                className="grow"
                rows={15}
                value={settings?.description ?? ""}
                placeholder="Enter a survey description here, which will be visible to all users who land on the survey page."
                onChange={(e) => setSettings({ ...settings, description: e.target.value } as QuizSettings)}
            >
            </textarea><br />
            <label>Access type</label>
            <select
                defaultValue={String(pollingTypeToString(settings.type))}
                onChange={(e) => {
                    setSettings({ ...settings, type: PollingType[e.target.selectedIndex] as keyof PollingType } as unknown as QuizSettings);
                }}
            >
                {
                    typesOfPolling.map((option, index) =>
                        <option key={index}>{pollingTypeToString(option)}</option>)
                }
            </select>
            {
                settings.type.toString() == PollingType[PollingType.OnlyAllowed].toString() ?
                <Button onClick={(_) => setShowAllowedUsers(true)}>Edit allowed users</Button>
                :
                <></>
            }
            <div className="buttons-wrapper">
                <Button variant="primary" onClick={saveSettings}>Save changes</Button>
            </div>
            <AllowedUsersEditor poll={props.quiz} show={showAllowedUsers} setShow={(isShow : boolean) => setShowAllowedUsers(isShow)} />
        </div>
    )
}


export default QuizGeneralSettings;


function pollingTypeToString(type: PollingType): String {
    switch (type.toString()) {
        case PollingType[PollingType.Anyone].toString():
            return "Anyone";
        case PollingType[PollingType.Authorized].toString():
            return "Authorized";
        case PollingType[PollingType.OnlyAllowed].toString():
            return "Only allowed";
        case PollingType[PollingType.OnlyOwner].toString():
            return "Only owner";
        default:
            throw new Error("Undefined mapped string value for this key of PollingType");
    }
}