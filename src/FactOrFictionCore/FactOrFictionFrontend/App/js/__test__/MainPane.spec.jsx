import React from 'react';
import expect from 'expect';
import { mount } from 'enzyme';
import MainPane from '../components/MainPane.jsx';
import { VIEW_INPUT, VIEW_RESULT } from '../constants/viewTypes';

describe('MainPane', () => {

    let mainPane;
    let addFact;
    let changeView;
    let view;
    let facts;
    
    beforeEach(() => {
        addFact = jest.fn();
        changeView = jest.fn();
        view = VIEW_INPUT;
        facts = ["hi"];
        mainPane = mount(
            <MainPane 
                addFact={addFact} 
                changeView={changeView} 
                facts={facts} 
                view={view} 
            />
        );
    });
    
    it('MainPane requires addFact prop', () => {
        expect(mainPane.props().addFact).toBeDefined();
    });

    it('MainPane requires changeView prop', () => {
        expect(mainPane.props().changeView).toBeDefined();
    });

    it('MainPane requires view prop', () => {
        expect(mainPane.props().view).toBeDefined();
    });

    it('MainPane requires facts prop', () => {
        expect(mainPane.props().facts).toBeDefined();
    });

    it('App renders nested components', () => {
        expect(mainPane.find('ListView').length).toEqual(1);
        expect(mainPane.find('InputPane').length).toEqual(1);
    });

    it('Button click calls changeView', () => {
        let button = mainPane.find('.change-view-button').first();
        button.simulate('click');
        expect(changeView).toBeCalledWith(VIEW_RESULT);
    });
});