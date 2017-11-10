import React from 'react';
import expect from 'expect';
import { mount } from 'enzyme';
import InputPane from '../components/InputPane.jsx';

describe('InputPane', () => {

    let inputPane;
    let fetchTextEntry;
    let changeView;    

    beforeEach(() => {
        fetchTextEntry = jest.fn();
        changeView = jest.fn();
        inputPane = mount(
            <InputPane fetchTextEntry={fetchTextEntry} changeView={changeView} />);
    });
    
    it('InputPane has a button', () => {
        expect(inputPane.find('button').text()).toEqual('Start');
    });

	it('InputPane has a textarea', () => {
		expect(inputPane.find('textarea').text()).toEqual('Enter some text (news article...)!');
	});

    it('InputPane requires changeView prop', () => {
        expect(inputPane.props().changeView).toBeDefined();
    });

    it('InputPane requires fetchTextEntry prop', () => {
        expect(inputPane.props().fetchTextEntry).toBeDefined();
    });

    it('Button click calls addText', () => {
        const button = inputPane.find('button').first();
        const input  = inputPane.find('textarea').first();
        input.simulate('change', { target: { value: 'Texting is easy as 1-2-3!' } });
        button.simulate('click');
        expect(fetchTextEntry).toBeCalledWith('Texting is easy as 1-2-3!');
        expect(changeView).toBeCalled();        
    });
});