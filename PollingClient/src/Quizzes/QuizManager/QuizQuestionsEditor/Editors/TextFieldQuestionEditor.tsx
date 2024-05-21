import React, { useState, ChangeEvent } from 'react';
import Button from 'react-bootstrap/Button';
import { TextFieldQuestion } from '../../../Questions/TextFieldQuestion';


const TextFieldQuestionEditor = (props: any) => {
    const [errorMsg, setErrorMsg] = useState<string>();
    const [question, setQuestion] = useState<TextFieldQuestion>(props.question);

    const handleInputChange = (field: keyof TextFieldQuestion) => (e: ChangeEvent<HTMLInputElement>) => {
        setQuestion(prevQuestion => ({
            ...prevQuestion,
            [field]: e.target.value,
        }));
    };

    async function save() {

        const questionsOrderRateUpdate = new Promise(async (resolve, reject) => {
            let response: Response | null = null;
            try {
                response = await fetch(`/api/poll/${props.poll.id}/question`, {
                    method: "PUT",
                    headers: {
                        "Accept": "application/json",
                        "Content-Type": "application/json",
                        "Authorization": "Bearer " + sessionStorage.getItem('token')
                    },
                    body: JSON.stringify(
                        { ...question }
                    )
                });
                if (response.status == 200)
                    resolve(response);
                else
                    reject(response);
            } catch {
                reject(response);
            }
        });
        questionsOrderRateUpdate.then(
            async (response: Response) => {
                props.onHide();
                props.onUpdate();
            },
            async (response: Response) => {
                let responseBody = await response.json();
                console.log(responseBody);
                if (responseBody.hasOwnProperty('errors')) {
                    let errorMessage = ''
                    for (const error in responseBody['errors']) {
                        for (const msg of responseBody['errors'][error]) {
                            errorMessage = errorMessage + msg + ". ";
                        }
                    }
                    setErrorMsg(errorMessage);
                } else {
                    setErrorMsg("An unknown error has occured");
                }
            }
        )
    };

    return (
        <div className="text-field-question-editor">
            <label htmlFor="field-name">Field name</label><br />
            <input
                id="field-name"
                placeholder="question name"
                value={question?.fieldName || ''}
                onChange={handleInputChange('fieldName')}
            /><br />
            <label htmlFor="field-description">Field description</label><br />
            <input
                id="field-description"
                placeholder="question description"
                value={question?.description || ''}
                onChange={handleInputChange('description')}
            /><br />
            <label htmlFor="field-placeholder">Placeholder</label><br />
            <input
                id="field-placeholder"
                placeholder="placeholder of text field"
                value={question?.fieldPlaceholder || ''}
                onChange={handleInputChange('fieldPlaceholder')}
            /><br />
            <p className="error">
                {errorMsg}
            </p>
            <div className="center editor-buttons-wrapper">
                <Button variant="dark" onClick={save} className="add-question-button">Save</Button>
            </div>
        </div>
    );
};

export default TextFieldQuestionEditor;
