import './QuizGeneralSettings.css';
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
    pollingType: PollingType
};


const QuizGeneralSettings = (props : any) => {
    const [settings, setSettings] = useState<QuizSettings>(props.quiz);

    const typesOfPolling = useMemo(() => {
        const values: string[] = Object.values(PollingType) as string[];
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
            <select onChange={(e) => setSettings({ ...settings, type: pollingTypeToString(e.target.value as keyof typeof PollingType) } as QuizSettings)}>
                {
                    typesOfPolling.map((option: string, index) =>
                        <option key={index}>{pollingTypeToString(option)}</option>)
                }
            </select>
            <div className="buttons-wrapper">
                <Button variant="primary" onClick={saveSettings}>Save changes</Button>
            </div>
        </div>
    )
}


export default QuizGeneralSettings;


function pollingTypeToString(type: string): String {
    switch (PollingType[type as keyof typeof PollingType]) {
        case PollingType.Anyone:
            return "Anyone";
        case PollingType.Authorized:
            return "Authorized";
        case PollingType.OnlyAllowed:
            return "Only allowed";
        case PollingType.OnlyOwner:
            return "Only owner";
        default:
            throw new Error("Undefined mapped string value for this key of PollingType");
    }
}