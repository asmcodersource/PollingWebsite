import React, { useState, ChangeEvent } from 'react';
import Button from 'react-bootstrap/Button';
import { putQuestionToServer } from './QuestionEditor'
import { TextFieldQuestion } from '../../../../../Questions/TextFieldQuestion';


const TextFieldQuestionEditor = (props: any) => {
    const [errorMsg, setErrorMsg] = useState<string>();
    const [question, setQuestion] = useState<TextFieldQuestion>(props.question);

    const handleInputChange = (field: keyof TextFieldQuestion) => (e: ChangeEvent<HTMLInputElement>) => {
        setQuestion(prevQuestion => ({
            ...prevQuestion,
            [field]: e.target.value,
        }));
    };

    props.saveButtonSignal.handler = save; // it saves his lexical envirement? I am so good... ( no I am not, wtf I am doing here? Anyway it works! Take your shit web development )

    async function save() {
        let sendQuestion = { ...question }
        let sendQuestionToServer = putQuestionToServer(sendQuestion, props.poll.id);
        sendQuestionToServer.then(
            () => {
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
        </div>
    );
};

export default TextFieldQuestionEditor;
